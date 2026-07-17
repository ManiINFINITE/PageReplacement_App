<div align="center">

### Operating Systems Project
# 🧠 Page Replacement Simulator

**Developed by Mani Vakili**

A .NET solution that implements and visualizes classical page replacement algorithms —
one shared engine, two completely different front ends: a retro terminal UI and a modern desktop dashboard.

</div>

---

## 📦 Repository Structure

This is a single .NET solution (`PRA.sln`) made of three projects, with a strict one-way dependency: both UIs depend on the engine, and the engine depends on nothing.

```
PRA.sln
├── PRA.Core     → the algorithm engine (no UI dependencies at all)
├── PRA.CLI      → Spectre.Console terminal UI
└── PRA.GUI      → Avalonia desktop UI (MVVM)
```

| Project | What it is | Key dependency |
|---|---|---|
| **PRA.Core** | Pure C# class library — algorithms, models, export utilities | *(none)* |
| **PRA.CLI** | Terminal simulator with a retro, bordered, keyboard-driven UI | [Spectre.Console](https://spectreconsole.net/) |
| **PRA.GUI** | Cross-platform desktop dashboard | [Avalonia UI](https://avaloniaui.net/) + CommunityToolkit.Mvvm |

Because both front ends talk to the exact same interface (`IPageReplacementAlgorithm`) and the exact same result type (`SimulationResult`), every algorithm behaves identically no matter which UI is driving it — and adding a new algorithm never requires touching either UI.

---

## 🧮 Algorithms Implemented

| Algorithm | Strategy | Realizable in a real OS? |
|---|---|---|
| **FIFO** | Evicts the page that's been resident the longest | ✅ Yes |
| **Optimal (Belady's MIN)** | Evicts the page used farthest in the future (or never again) | ❌ No — requires future knowledge; used only as the theoretical best-case benchmark |
| **LRU** | Evicts the least recently used page | ✅ Yes (with cost) |
| **LFU** | Evicts the least frequently used page, ties broken by least-recently-used | ✅ Yes |
| **Clock (Second-Chance)** | A circular reference-bit sweep — the cheap, practical approximation of LRU that real kernels actually use | ✅ Yes |

Every algorithm is a self-contained class implementing one shared interface, so they're 100% interchangeable across every feature below — comparisons, exports, and both UIs.

---

## ✨ Features

### Shared engine (PRA.Core)
- **Five full algorithm implementations** with correct handling of empty frames, hits, faults, and evictions.
- **Step-by-step simulation history** — every reference produces a full snapshot (`SimulationStep`) of frame state, hit/fault outcome, and the evicted page, so any UI can scrub backward and forward through a run instantly with zero recomputation.
- **Algorithm-specific visualization data** — Clock steps carry reference bits and the clock-hand pointer position; LFU steps carry per-frame frequency counts.
- **CSV & Markdown export**, for both a single run and a full multi-algorithm comparison, written to a predictable `exports/` folder at the solution root regardless of which project or working directory launched it.
- **Zero UI dependencies** — fully unit-testable, fully reusable by any future front end (web, mobile, whatever).

### 🖥️ Terminal UI (PRA.CLI)
- **Retro boot-style title screen** and a fully bordered, keyboard-navigated main menu (arrow keys + Enter — no raw text prompts).
- **New single simulation** — pick an algorithm, enter a reference string and frame count, then step through frame-by-frame with:
  - a live reference-string track (past pages color-coded hit/fault, current page highlighted, future pages dimmed)
  - a horizontal frame-state panel (color-coded hit / fault / replaced, plus Clock's reference bits & pointer when applicable)
  - a running statistics panel (hits, faults, hit ratio) with a breakdown chart
  - a full history matrix (reference string × frames, textbook-style) toggleable on demand
- **Compare all algorithms** — runs every algorithm on the same input at once, ranks them by hit ratio in a table plus a color-coded breakdown chart, then lets you drop into a full step-through of any one of them.
- **Compare two algorithms side-by-side** — pick any two algorithms and step through both **in lockstep**, in two columns, with arrow keys driving both simulations simultaneously.
- **Export to CSV or Markdown** directly from any viewer (single-run or full comparison), with a bordered file-name prompt.
- **Configurable accent color** — a Settings screen lets you pick from a curated palette of accent colors with a live preview; the entire terminal theme (borders, highlights, hit/fault colors) re-themes from that single choice.

### 🖥️ Desktop UI (PRA.GUI)
- **Modern dashboard layout** — sidebar navigation, stat cards, a reference-string track, a frame-state visualization, and a scrollable history grid, all built from a shared card/typography/button design system.
- **Collapsible sidebar** and a **light/dark theme toggle**, both live-switchable without restarting the app.
- **New Simulation** as an overlay dialog — pick an algorithm from the same shared catalog used everywhere else, type a reference string and frame count, with inline validation errors instead of silent failures.
- **Compare All Algorithms** page — same ranked comparison as the CLI, rendered as cards/rows with a consistent per-algorithm color assignment that stays stable even as results get re-sorted.
- **Compare Two Algorithms** page — two independent dashboards running side by side, with a single **"step both"** command advancing both simulations together.
- **Export dialogs** for both single runs and comparisons, reusing the exact same `PRA.Core` CSV/Markdown writers as the CLI — output is byte-for-byte consistent between both UIs.
- **Overlay-based dialog system and cached navigation** — pages are built once and reused on revisit rather than being torn down and rebuilt, so switching between Dashboard / Compare All / Compare Two is instant.

---

## 🚀 Getting Started

### Prerequisites

You need the **.NET 10 SDK**. Get it from https://dotnet.microsoft.com/download/dotnet/10.0, or:

```bash
# Windows (winget)
winget install Microsoft.DotNet.SDK.10

# macOS (Homebrew)
brew install --cask dotnet-sdk

# Linux (Debian/Ubuntu)
sudo apt-get install -y dotnet-sdk-10.0
```

Verify with `dotnet --version` — it should print `10.x.x`.

### Clone & build

```bash
git clone <repository-url>
cd PageReplacement_App
dotnet restore
dotnet build
```

### Run the terminal UI

```bash
dotnet run --project PRA.CLI
```

| Key | Action |
|---|---|
| `↑` `↓` | Move menu selection |
| `←` `→` | Step backward / forward |
| `Home` `End` | Jump to first / last step |
| `T` | Toggle the full history table |
| `E` | Export current result (CSV / Markdown) |
| `Enter` | Confirm / select |
| `Esc` | Back |

### Run the desktop UI

```bash
dotnet run --project PRA.GUI
```

Cross-platform out of the box — the same command works on Windows, macOS, and Linux.

---

## 🏗️ Architecture

```
                ┌────────────────────┐
                │      PRA.Core       │
                │  algorithms · models │
                │      · utilities     │
                └──────────▲──────────┘
                           │
              ┌────────────┴────────────┐
              │                         │
     ┌────────┴────────┐      ┌─────────┴────────┐
     │     PRA.CLI       │      │     PRA.GUI       │
     │ Spectre.Console TUI│      │ Avalonia desktop   │
     └────────────────────┘      └────────────────────┘
```

`PRA.Core` defines one interface — `IPageReplacementAlgorithm` — and every algorithm is a Strategy-pattern implementation of it. Both UIs iterate the same `AlgorithmCatalog` and call the same `Run(referenceString, frameCount)` method, receiving back an immutable `SimulationResult` full of `SimulationStep` snapshots. Neither UI contains any simulation logic of its own — they only gather input, call into `PRA.Core`, and render the result.

For a full breakdown of *why* every class in `PRA.Core` exists — the algorithms, the models, the design decisions behind them — see [`report.pdf`](./report.pdf) (or the LaTeX source, [`report.tex`](./report.tex)).

---

## 📤 Exports

Exporting a result (from either UI) writes a `.csv` or `.md` file into an `exports/` folder created at the solution root — found automatically by walking up from the working directory until a `.sln` file is located, so it always lands in the same place no matter which project you ran or which directory you launched it from.

- **Single-run export** — full step-by-step table (step, page, per-frame state, hit/fault, replaced page) plus summary stats.
- **Comparison export** — one summary table across every algorithm, sorted by hit ratio.

---

## 🎨 Design Philosophy

- **One engine, many faces.** All simulation logic lives in `PRA.Core` and nowhere else — both UIs are thin, swappable presentation layers over the exact same algorithms.
- **Immutability.** Simulation steps and results are built once and never mutated afterward, so history scrubbing, side-by-side comparison, and export can all safely share the same data.
- **Open/Closed.** New algorithms (LRU and LFU were added well after FIFO/Optimal/Clock already existed) require zero changes to any UI — only a new class and one catalog entry.

---

<div align="center">

*Operating Systems — Page Replacement Simulator*
*Mani Vakili — 40231996*

</div>
