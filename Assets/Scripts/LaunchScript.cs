using UnityEngine;
using TMPro;
using Env0.Terminal;

namespace Env0.Terminal
{
    public class TrueTerminalUnity : MonoBehaviour
    {
        public TMP_Text TerminalOutput;
        private TerminalEngineAPI _api;
        private TerminalRenderState _state;
        private string terminalBuffer = "";
        private string currentInput = "";
        private bool playerInputAllowed = false;
        private bool initFailed = false;

        private void Start()
        {
            DebugUtility.Enabled = true; // Enable early for error logs

            try
            {
                _api = new TerminalEngineAPI();
                _api.Initialize();
                initFailed = false;

                _state = _api.Execute(""); // Boot phase
                AppendRenderState(_state);
                UpdateTerminalOutput();
            }
            catch (System.Exception ex)
            {
                initFailed = true;
                terminalBuffer = "[FATAL] Terminal failed to initialize:\n" + ex.Message;
                UpdateTerminalOutput();
                Debug.LogError("[FATAL] Terminal failed to initialize: " + ex);
            }
        }

        private void Update()
        {
            if (initFailed) return;

            // If we're waiting for user input (login prompt, password prompt, or in the terminal phase)
            if (Input.anyKeyDown && playerInputAllowed)
            {
                foreach (var c in Input.inputString)
                {
                    if (c == '\b') // Backspace
                    {
                        if (currentInput.Length > 0)
                            currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    }
                    else if (c == '\n' || c == '\r') // Enter
                    {
                        // Show what the user typed at the prompt
                        terminalBuffer += _state.Prompt + currentInput + "\n";
                        _state = _api.Execute(currentInput);
                        AppendRenderState(_state);

                        // After executing, check if we are still allowed to type (based on phase)
                        playerInputAllowed = (
                            _state.Phase == TerminalPhase.Terminal ||
                            _state.IsLoginPrompt ||
                            _state.IsPasswordPrompt
                        );

                        currentInput = "";
                    }
                    else if (c >= ' ')
                    {
                        currentInput += c;
                    }
                }

                UpdateTerminalOutput();
            }
            else if (Input.anyKeyDown && !playerInputAllowed)
            {
                // In phases where input "advances" (e.g., press any key to continue after boot)
                _state = _api.Execute("");
                AppendRenderState(_state);

                playerInputAllowed = (
                    _state.Phase == TerminalPhase.Terminal ||
                    _state.IsLoginPrompt ||
                    _state.IsPasswordPrompt
                );
                UpdateTerminalOutput();
            }
        }

        private void AppendRenderState(TerminalRenderState state)
        {
            if (initFailed) return;

            // Boot sequence
            if (state.Phase == TerminalPhase.Booting)
            {
                if (state.BootSequenceLines != null)
                {
                    foreach (var line in state.BootSequenceLines)
                        terminalBuffer += line + "\n";
                }
            }
            // Login prompt / password prompt
            else if (state.Phase == TerminalPhase.Login)
            {
                if (state.OutputLines != null && state.OutputLines.Count > 0)
                {
                    foreach (var line in state.OutputLines)
                        terminalBuffer += line.Text + "\n";
                }
            }
            // Terminal phase (command output)
            else if (state.Phase == TerminalPhase.Terminal)
            {
                if (state.OutputLines != null && state.OutputLines.Count > 0)
                {
                    foreach (var line in state.OutputLines)
                        terminalBuffer += line.Text + "\n";
                }
            }
        }

        private void UpdateTerminalOutput()
        {
            if (initFailed)
            {
                TerminalOutput.text = terminalBuffer;
                return;
            }

            if (playerInputAllowed)
            {
                // Show the prompt + input + fake caret at the end
                TerminalOutput.text = terminalBuffer + _state.Prompt + currentInput + "<color=#ffff00>|</color>";
            }
            else
            {
                // Show just the buffer (no input line)
                TerminalOutput.text = terminalBuffer;
            }
        }
    }
}