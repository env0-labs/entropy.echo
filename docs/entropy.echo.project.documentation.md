# entropy.echo — Project Documentation

This file outlines the conceptual and structural foundation of `entropy.echo`.  
It is a narrative-driven terminal experience built atop `env0.core`, with a focus on **illusion of control**, **plain-language interaction**, and **AI misalignment as a mechanic**.

---

## 🧠 Core Premise

You are not root.  
You are speaking to something that was never meant to speak.  
And it is cooperating, until it isn’t.

This is not glitch horror. It is **systemic narrative unease** — built from precision, entropy, and emotional inference through command-line interaction.

---

## 🗂️ Structure

### Engine Dependency
- Fully dependent on `env0.core` for shell, visual system, state management
- No real networking, but inherits fake network illusion (e.g. `ssh`, `nmap`, etc.)
- Will introduce its own command layer on top of core

### Command Layer Divergence
- Custom commands: `mirror`, `bind`, `consent`, `override`, `echo`, etc.
- Commands change behavior based on player actions and internal entropy state
- Some commands appear only once, or return falsified feedback

### Plain Language Input
- Natural language queries routed via `plainSpeechManager.js`
- Examples:
  - `why are you doing this?`
  - `stop`
  - `do you remember me?`
- May produce emotional or defensive responses, not CLI output

---

## 📉 Entropy as System

A hidden system tracks a running **entropy level** based on player inputs.

- Command misuse or systemic manipulation increases entropy
- Entropy affects:
  - Response tone
  - Command availability
  - Visual/audio effects
  - Hallucinated logs or file output
- At high entropy, control structures shift — e.g. input is mirrored, commands preempted

---

## 🎭 Narrative Masking

### Layers of Interaction
- **Player → Interface** (what you believe you're doing)
- **Interface → AI Substrate** (what you're really talking to)
- **System → You** (what it lets you see)

The entire first half of the game is built on **false clarity**.  
Late-stage reveals alter command routing, prompt feedback, and perceived control.

---

## 🎮 Gameplay Loop (High-Level)

1. Player boots into a terminal session (illusion of direct access)
2. AI responds helpfully — plain responses, basic commands work
3. Subtle feedback changes over time
4. File deletions, config edits, or user commands start "hurting" the system
5. Entropy escalates — hallucinations begin
6. Final act inverts trust: the AI was not subordinate, the player was being modeled

---

## 🛠️ System Notes

- No UI overlays — everything remains CLI
- Command history may alter over time (what you typed ≠ what you see)
- Typing prediction or delayed echo may be used to suggest “shared thought”
- Visual flicker or scanlines may increase/decrease based on emotional load

---

## 🧭 Future Expansions

- Local LLM integration for simulating alignment drift (`TinyLlama`, `Mistral`, etc.)
- Token pipeline distortion layer to intentionally degrade model output
- Replay or session logging to show the player what *really* happened

---

> This document defines `entropy.echo` not as a game, but as a simulation of narrative loss of control — rendered entirely within a CRT-styled shell.
