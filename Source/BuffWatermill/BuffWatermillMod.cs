using System.Globalization;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace BuffWatermill
{
    public sealed class BuffWatermillSettings : ModSettings
    {
        public const float DefaultMultiplier = 1.5f;

        public float multiplier = DefaultMultiplier;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref multiplier, "multiplier", DefaultMultiplier);
            multiplier = Mathf.Clamp(multiplier, BuffWatermillMod.MinimumMultiplier, BuffWatermillMod.MaximumMultiplier);
        }
    }

    public sealed class BuffWatermillMod : Mod
    {
        internal const float MinimumMultiplier = 0.5f;
        internal const float MaximumMultiplier = 5f;

        private const float RowHeight = 30f;
        private const float ButtonWidth = 120f;
        private const float Gap = 10f;

        private readonly BuffWatermillSettings settings;
        private float pendingMultiplier;

        private static CompProperties_Power watermillPower;
        private static readonly FieldInfo BasePowerConsumptionField = typeof(CompProperties_Power).GetField(
            "basePowerConsumption",
            BindingFlags.Instance | BindingFlags.NonPublic);
        private static float originalPowerConsumption;
        private static bool originalPowerCaptured;

        public BuffWatermillMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<BuffWatermillSettings>();
            pendingMultiplier = settings.multiplier;
            LongEventHandler.ExecuteWhenFinished(ApplySavedSetting);
        }

        public override string SettingsCategory()
        {
            return "Buff Watermill";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect sliderRect = new Rect(inRect.x, inRect.y, inRect.width, RowHeight);
            string value = pendingMultiplier.ToString("0.0", CultureInfo.InvariantCulture) + "x";
            pendingMultiplier = Widgets.HorizontalSlider(
                sliderRect,
                pendingMultiplier,
                MinimumMultiplier,
                MaximumMultiplier,
                false,
                "Watermill power multiplier: " + value,
                MinimumMultiplier.ToString("0.0") + "x",
                MaximumMultiplier.ToString("0.0") + "x",
                0.1f);

            float buttonsY = sliderRect.yMax + Gap;
            Rect defaultsRect = new Rect(inRect.x, buttonsY, ButtonWidth, RowHeight);
            Rect applyRect = new Rect(defaultsRect.xMax + Gap, buttonsY, ButtonWidth, RowHeight);

            if (Widgets.ButtonText(defaultsRect, "Default"))
            {
                pendingMultiplier = BuffWatermillSettings.DefaultMultiplier;
            }

            if (Widgets.ButtonText(applyRect, "Apply"))
            {
                settings.multiplier = pendingMultiplier;
                ApplyMultiplier(settings.multiplier);
                WriteSettings();
            }
        }

        private void ApplySavedSetting()
        {
            ApplyMultiplier(settings.multiplier);
        }

        private static void ApplyMultiplier(float multiplier)
        {
            ThingDef watermill = DefDatabase<ThingDef>.GetNamedSilentFail("WatermillGenerator");
            if (watermill == null)
            {
                Log.Error("[Buff Watermill] Could not find the WatermillGenerator ThingDef.");
                return;
            }

            CompProperties_Power power = watermill.GetCompProperties<CompProperties_Power>();
            if (power == null)
            {
                Log.Error("[Buff Watermill] Could not find the watermill power properties.");
                return;
            }

            if (BasePowerConsumptionField == null)
            {
                Log.Error("[Buff Watermill] Could not find RimWorld's base power consumption field.");
                return;
            }

            if (!originalPowerCaptured || watermillPower != power)
            {
                watermillPower = power;
                originalPowerConsumption = (float)BasePowerConsumptionField.GetValue(power);
                originalPowerCaptured = true;
            }

            BasePowerConsumptionField.SetValue(power, originalPowerConsumption * multiplier);
        }
    }
}
