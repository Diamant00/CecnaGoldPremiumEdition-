using Content.Client.Audio;
using Content.Shared._White;
using Content.Shared.CCVar;
using Robust.Client.Audio;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;
using Range = Robust.Client.UserInterface.Controls.Range;

namespace Content.Client.Options.UI.Tabs
{
    [GenerateTypedNameReferences]
    public sealed partial class AudioTab : Control
    {
        [Dependency] private readonly IConfigurationManager _cfg = default!;
        private readonly IAudioManager _audio;

        public AudioTab()
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);

            _audio = IoCManager.Resolve<IAudioManager>();
            LobbyMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            RestartSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            EventMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.EventMusicEnabled);
            AdminSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AdminSoundsEnabled);

            ApplyButton.OnPressed += OnApplyButtonPressed;
            ResetButton.OnPressed += OnResetButtonPressed;

            AttachUpdateChangesHandler(
                MasterVolumeSlider,
                MidiVolumeSlider,
                AmbientMusicVolumeSlider,
                AmbienceVolumeSlider,
                AmbienceSoundsSlider,
                LobbyVolumeSlider,
                InterfaceVolumeSlider,
                AnnouncerVolumeSlider,
                TtsVolumeSlider, // WD EDIT

                LobbyMusicCheckBox,
                RestartSoundsCheckBox,
                EventMusicCheckBox,
                AnnouncerDisableMultipleSoundsCheckBox,
                AdminSoundsCheckBox
            );

            AmbienceSoundsSlider.MinValue = _cfg.GetCVar(CCVars.MinMaxAmbientSourcesConfigured);
            AmbienceSoundsSlider.MaxValue = _cfg.GetCVar(CCVars.MaxMaxAmbientSourcesConfigured);

            Reset();
            return;

            void AttachUpdateChangesHandler(params Control[] controls)
            {
                foreach (var control in controls)
                {
                    switch (control)
                    {
                        case Slider slider:
                            slider.OnValueChanged += _ => UpdateChanges();
                            break;
                        case CheckBox checkBox:
                            checkBox.OnToggled += _ => UpdateChanges();
                            break;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            ApplyButton.OnPressed -= OnApplyButtonPressed;
            ResetButton.OnPressed -= OnResetButtonPressed;

            DetachUpdateChangesHandler(
                MasterVolumeSlider,
                MidiVolumeSlider,
                AmbientMusicVolumeSlider,
                AmbienceVolumeSlider,
                AmbienceSoundsSlider,
                LobbyVolumeSlider,
                InterfaceVolumeSlider,
                AnnouncerVolumeSlider,
                TtsVolumeSlider, // WD EDIT

                LobbyMusicCheckBox,
                RestartSoundsCheckBox,
                EventMusicCheckBox,
                AnnouncerDisableMultipleSoundsCheckBox,
                AdminSoundsCheckBox
            );

            base.Dispose(disposing);
            return;

            void DetachUpdateChangesHandler(params Control[] controls)
            {
                foreach (var control in controls)
                {
                    switch (control)
                    {
                        case Slider slider:
                            slider.OnValueChanged -= _ => UpdateChanges();
                            break;
                        case CheckBox checkBox:
                            checkBox.OnToggled -= _ => UpdateChanges();
                            break;
                    }
                }
            }
        }


        private void OnApplyButtonPressed(BaseButton.ButtonEventArgs args)
        {
            _cfg.SetCVar(CVars.AudioMasterVolume, MasterVolumeSlider.Value / 100f * ContentAudioSystem.MasterVolumeMultiplier);
            // Want the CVar updated values to have the multiplier applied
            // For the UI we just display 0-100 still elsewhere
            _cfg.SetCVar(CVars.MidiVolume, MidiVolumeSlider.Value / 100f * ContentAudioSystem.MidiVolumeMultiplier);
            _cfg.SetCVar(CCVars.AmbienceVolume, AmbienceVolumeSlider.Value / 100f * ContentAudioSystem.AmbienceMultiplier);
            _cfg.SetCVar(CCVars.AmbientMusicVolume, AmbientMusicVolumeSlider.Value / 100f * ContentAudioSystem.AmbientMusicMultiplier);
            _cfg.SetCVar(CCVars.LobbyMusicVolume, LobbyVolumeSlider.Value / 100f * ContentAudioSystem.LobbyMultiplier);
            _cfg.SetCVar(CCVars.InterfaceVolume, InterfaceVolumeSlider.Value / 100f * ContentAudioSystem.InterfaceMultiplier);
            _cfg.SetCVar(CCVars.AnnouncerVolume, AnnouncerVolumeSlider.Value / 100f * ContentAudioSystem.AnnouncerMultiplier);
            _cfg.SetCVar(WhiteCVars.TTSVolume, TtsVolumeSlider.Value  / 100f * ContentAudioSystem.TTSMultiplier); // WD EDIT

            _cfg.SetCVar(CCVars.MaxAmbientSources, (int)AmbienceSoundsSlider.Value);

            _cfg.SetCVar(CCVars.LobbyMusicEnabled, LobbyMusicCheckBox.Pressed);
            _cfg.SetCVar(CCVars.RestartSoundsEnabled, RestartSoundsCheckBox.Pressed);
            _cfg.SetCVar(CCVars.EventMusicEnabled, EventMusicCheckBox.Pressed);
            _cfg.SetCVar(CCVars.AnnouncerDisableMultipleSounds, AnnouncerDisableMultipleSoundsCheckBox.Pressed);
            _cfg.SetCVar(CCVars.AdminSoundsEnabled, AdminSoundsCheckBox.Pressed);
            _cfg.SaveToFile();
            UpdateChanges();
        }

        private void OnResetButtonPressed(BaseButton.ButtonEventArgs args)
        {
            Reset();
        }

        private void Reset()
        {
            MasterVolumeSlider.Value = _cfg.GetCVar(CVars.AudioMasterVolume) * 100f / ContentAudioSystem.MasterVolumeMultiplier;
            MidiVolumeSlider.Value = _cfg.GetCVar(CVars.MidiVolume) * 100f / ContentAudioSystem.MidiVolumeMultiplier;
            AmbienceVolumeSlider.Value = _cfg.GetCVar(CCVars.AmbienceVolume) * 100f / ContentAudioSystem.AmbienceMultiplier;
            AmbientMusicVolumeSlider.Value = _cfg.GetCVar(CCVars.AmbientMusicVolume) * 100f / ContentAudioSystem.AmbientMusicMultiplier;
            LobbyVolumeSlider.Value = _cfg.GetCVar(CCVars.LobbyMusicVolume) * 100f / ContentAudioSystem.LobbyMultiplier;
            InterfaceVolumeSlider.Value = _cfg.GetCVar(CCVars.InterfaceVolume) * 100f / ContentAudioSystem.InterfaceMultiplier;
            AnnouncerVolumeSlider.Value = _cfg.GetCVar(CCVars.AnnouncerVolume) * 100f / ContentAudioSystem.AnnouncerMultiplier;
            TtsVolumeSlider.Value = _cfg.GetCVar(WhiteCVars.TTSVolume) * 100f / ContentAudioSystem.TTSMultiplier; // WD EDIT

            AmbienceSoundsSlider.Value = _cfg.GetCVar(CCVars.MaxAmbientSources);

            LobbyMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            RestartSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            EventMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.EventMusicEnabled);
            AnnouncerDisableMultipleSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AnnouncerDisableMultipleSounds);
            AdminSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            UpdateChanges();
        }

        private void UpdateChanges()
        {
            // y'all need jesus.
            var isMasterVolumeSame =
                Math.Abs(MasterVolumeSlider.Value - _cfg.GetCVar(CVars.AudioMasterVolume) * 100f / ContentAudioSystem.MasterVolumeMultiplier) < 0.01f;
            var isMidiVolumeSame =
                Math.Abs(MidiVolumeSlider.Value - _cfg.GetCVar(CVars.MidiVolume) * 100f / ContentAudioSystem.MidiVolumeMultiplier) < 0.01f;
            var isAmbientVolumeSame =
                Math.Abs(AmbienceVolumeSlider.Value - _cfg.GetCVar(CCVars.AmbienceVolume) * 100f / ContentAudioSystem.AmbienceMultiplier) < 0.01f;
            var isAmbientMusicVolumeSame =
                Math.Abs(AmbientMusicVolumeSlider.Value - _cfg.GetCVar(CCVars.AmbientMusicVolume) * 100f / ContentAudioSystem.AmbientMusicMultiplier) < 0.01f;
            var isLobbyVolumeSame =
                Math.Abs(LobbyVolumeSlider.Value - _cfg.GetCVar(CCVars.LobbyMusicVolume) * 100f / ContentAudioSystem.LobbyMultiplier) < 0.01f;
            var isInterfaceVolumeSame =
                Math.Abs(InterfaceVolumeSlider.Value - _cfg.GetCVar(CCVars.InterfaceVolume) * 100f / ContentAudioSystem.InterfaceMultiplier) < 0.01f;
            var isAnnouncerVolumeSame =
                Math.Abs(AnnouncerVolumeSlider.Value - _cfg.GetCVar(CCVars.AnnouncerVolume) * 100f / ContentAudioSystem.AnnouncerMultiplier) < 0.01f;
            var isTtsVolumeSame =
                Math.Abs(TtsVolumeSlider.Value - _cfg.GetCVar(WhiteCVars.TTSVolume) * 100f / ContentAudioSystem.TTSMultiplier) < 0.01f; // WD EDIT

            var isAmbientSoundsSame = (int)AmbienceSoundsSlider.Value == _cfg.GetCVar(CCVars.MaxAmbientSources);
            var isLobbySame = LobbyMusicCheckBox.Pressed == _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            var isRestartSoundsSame = RestartSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            var isEventSame = EventMusicCheckBox.Pressed == _cfg.GetCVar(CCVars.EventMusicEnabled);
            var isAnnouncerDisableMultipleSoundsSame = AnnouncerDisableMultipleSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.AnnouncerDisableMultipleSounds);
            var isAdminSoundsSame = AdminSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            var isEverythingSame = isMasterVolumeSame && isMidiVolumeSame && isAmbientVolumeSame
                && isAmbientMusicVolumeSame && isAmbientSoundsSame && isLobbySame && isRestartSoundsSame && isEventSame
                && isAnnouncerDisableMultipleSoundsSame && isAdminSoundsSame && isLobbyVolumeSame
                && isInterfaceVolumeSame && isAnnouncerVolumeSame && isTtsVolumeSame; // WD EDIT
            ApplyButton.Disabled = isEverythingSame;
            ResetButton.Disabled = isEverythingSame;
            MasterVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", MasterVolumeSlider.Value / 100));
            MidiVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", MidiVolumeSlider.Value / 100));
            AmbientMusicVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AmbientMusicVolumeSlider.Value / 100));
            AmbienceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AmbienceVolumeSlider.Value / 100));
            LobbyVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", LobbyVolumeSlider.Value / 100));
            InterfaceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", InterfaceVolumeSlider.Value / 100));
            AnnouncerVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AnnouncerVolumeSlider.Value / 100));
            TtsVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsVolumeSlider.Value / 100)); // WD EDIT
            AmbienceSoundsLabel.Text = ((int)AmbienceSoundsSlider.Value).ToString();
        }
    }
}
