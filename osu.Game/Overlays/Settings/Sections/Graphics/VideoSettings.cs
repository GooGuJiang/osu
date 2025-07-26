// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Video;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Desktop;
using osu.Game.Localisation;

namespace osu.Game.Overlays.Settings.Sections.Graphics
{
    public partial class VideoSettings : SettingsSubsection
    {
        protected override LocalisableString Header => GraphicsSettingsStrings.VideoHeader;

        private Bindable<HardwareVideoDecoder> hardwareVideoDecoder;
        private SettingsCheckbox hwAccelCheckbox;
        private SettingsCheckbox dlssCheckbox;

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, OsuConfigManager osuConfig)
        {
            hardwareVideoDecoder = config.GetBindable<HardwareVideoDecoder>(FrameworkSetting.HardwareVideoDecoder);
            var useDlss = osuConfig.GetBindable<bool>(OsuSetting.UseDLSS);

            Children = new Drawable[]
            {
                hwAccelCheckbox = new SettingsCheckbox
                {
                    LabelText = GraphicsSettingsStrings.UseHardwareAcceleration,
                },
                dlssCheckbox = new SettingsCheckbox
                {
                    LabelText = GraphicsSettingsStrings.EnableDlss,
                    Current = useDlss,
                    CanBeShown = { Value = DLSSManager.Available },
                },
            };

            hwAccelCheckbox.Current.Default = hardwareVideoDecoder.Default != HardwareVideoDecoder.None;
            hwAccelCheckbox.Current.Value = hardwareVideoDecoder.Value != HardwareVideoDecoder.None;

            hwAccelCheckbox.Current.BindValueChanged(val =>
            {
                hardwareVideoDecoder.Value = val.NewValue ? HardwareVideoDecoder.Any : HardwareVideoDecoder.None;
            });

            dlssCheckbox.Current.BindValueChanged(val =>
            {
                DLSSManager.Enabled = val.NewValue;
            }, true);
        }
    }
}
