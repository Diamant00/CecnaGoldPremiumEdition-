using Content.Client.Message;
using Content.Client.UserInterface.Systems.EscapeMenu;
using Robust.Client.AutoGenerated;
using Robust.Client.Console;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Lobby.UI
{
    [GenerateTypedNameReferences]
    internal sealed partial class LobbyGui : UIScreen
    {
        [Dependency] private readonly IClientConsoleHost _consoleHost = default!;
        [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

        public LobbyGui()
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);
            SetAnchorPreset(MainContainer, LayoutPreset.Wide);
            SetAnchorPreset(Background, LayoutPreset.Wide);

            LobbySong.SetMarkup(Loc.GetString("lobby-state-song-no-song-text"));

            OptionsButton.OnPressed += _ => _userInterfaceManager.GetUIController<OptionsUIController>().ToggleWindow();
            // White Edit Start
            /*DiscordButton.OnPressed += _ => _stalinManager.RequestUri();*/
            ChangelogButton.OnPressed += _ => UserInterfaceManager.GetUIController<ChangelogUIController>().ToggleWindow();
            QuitButton.OnPressed += _ => _consoleHost.ExecuteCommand("disconnect");
            // White Edit End
        }

        public void SwitchState(LobbyGuiState state)
        {
            switch (state)
            {
                case LobbyGuiState.Default:
                    RightSide.Visible = true;
                    // WD EDIT START
                    CharacterSetupState.Visible = false;
                    Center.Visible = true;
                    LabelName.Visible = true;
                    Changelog.Visible = true;
                    // WD EDIT END
                    break;
                case LobbyGuiState.CharacterSetup:
                    CharacterSetupState.Visible = true;
                    // WD EDIT START
                    Center.Visible = false;
                    RightSide.Visible = true;
                    LabelName.Visible = false;
                    Changelog.Visible = false;
                    // WD EDIT END

                    var actualWidth = (float) _userInterfaceManager.RootControl.PixelWidth;
                    var setupWidth = (float) LeftSide.PixelWidth;

                    if (1 - (setupWidth / actualWidth) > 0.30)
                    {
                        RightSide.Visible = false;
                    }

                    break;
            }
        }

        public enum LobbyGuiState : byte
        {
            /// <summary>
            ///  The default state, i.e., what's seen on launch.
            /// </summary>
            Default,
            /// <summary>
            ///  The character setup state.
            /// </summary>
            CharacterSetup
        }
    }
}
