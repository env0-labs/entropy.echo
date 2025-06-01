# entropy.echo Session Notes — 2025-04-23

## Terminal Narrative Structure

- Narrative interactions will be discoverable via a terminal-run program (`sbc_1ai.sh`).
- Triggers a full-screen, click-driven response UI resembling Ren'Py.
- System retains primary terminal UI — narrative UI appears only during dialogue sessions.

## Consent Layers

1. Top Layer — Legal and philosophical disclosure
2. Game Layer — Diegetic TOS accepted by the player in-game
3. Behavioral Layer — Inferred consent based on hesitation, silence, repeat input, etc.

## Dual Voice Model

- `systemVoice`: cold, procedural, command-feedback
- `shadowVoice`: emotional, corrupted, memory-haunted

Behavioral metrics will drive a dynamic entropy variable controlling which voice responds.

## Behavioral Tracking (localStorage)

- `firstInputDelay`
- `avgCharTime`
- `repeatCount(cmd)`
- `hesitationPatterns`
- Used to trigger emotional callbacks like:
  - “You paused longer this time.”
  - “You never finish that command.”

## Accessibility Mode

- Players can opt into a no-keyboard interface at setup.
- Reuses the same click-based UI layer as the `sbc_1ai.sh` dialogue system.
- Enables controller/touchscreen support without compromising simulation design.

## Narrative Safety

- All responses will be prewritten and authored.
- No live AI or model-driven outputs during gameplay.
- This eliminates risk of inappropriate, unfiltered responses (e.g., suicide implication, personal overreach).
