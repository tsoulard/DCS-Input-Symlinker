# DcsSymlinker
A cross-platform .NET tool for managing symbolic links for DCS World input configuration files.
This application automatically detects your DCS "Saved Games" directory (supporting Windows, Steam on Linux, and Bottles on Linux) and manages input device configuration files (Joysticks, Keyboards, Mice, TrackIR).

## Features

- Cross-Platform Support: Automatically detects DCS paths on Windows, Steam Proton (Linux), and Bottles (Linux).
- Steam Library Detection: Parses `libraryfolders.vdf` to find game installations on Linux.
- Automated Symlinking: Scans for `.diff.lua` files and manages symlinks for device configurations.
- Filtering: Filter by device type (Joystick, Keyboard, Mouse, TrackIR) or by creation date.
- CLI Interface: Built with Spectre.Console for a clean and interactive command-line experience.

## Prerequisites

- .NET 10.0 SDK (This project targets `net10.0`).
- DCS World installed.

## Installation

Clone the repository and restore dependencies:

## How it Works

1. Path Detection:
   - Windows: Looks in `UserProfile/Saved Games/DCS/Config/Input`.
   - Linux: Parses Steam's `libraryfolders.vdf` for the app ID `223750`. It then constructs the path inside the `compatdata` folder.
   - Bottles: Checks `~/.var/app/com.usebottles/bottles/data/bottles/data.yml` for a custom path.

2. File Processing:
   - The tool scans subdirectories for `.diff.lua` files.
   - It parses filenames (e.g., `Joystick {12345}.diff.lua`) to identify unique device IDs.
   - It creates or updates symbolic links based on the detected files.

## Usage
### Command Line Options

| Option | Alias | Description | Default |
| :--- | :--- | :--- | :--- |
| `--path` | `-p` | Manually specify the path to your DCS Saved Games directory. | Auto-detected |
| `--folder` | `-f` | Which folders to look in to create symlinks. | `Joystick` |
| `--latest-ids` | `-l` | Only look for IDs on files created in the last day. | `false` |
| `--clean` | `-c` | Remove all existing symlinks before creating new ones. | `false` |