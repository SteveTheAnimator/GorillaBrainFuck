![GBF](https://raw.githubusercontent.com/SteveTheAnimator/GorillaBrainFuck/master/Media/gbf.png)

Gorilla BrainFuck is a mod that adds a BrainFuck code interpreter to Gorilla Tag. BrainFuck is an esoteric programming language with just eight commands, designed to be minimalistic yet Turing complete.

## Features

- In-game BrainFuck interpreter with GUI
- Real-time code execution
- Two execution modes: Result and Output view
- Draggable interface window
- 30,000 cell memory capacity

## Installation

1. Ensure you have BepInEx installed in your Gorilla Tag directory
2. Download the latest release of Gorilla BrainFuck
3. Extract the DLL file to your Gorilla Tag's `BepInEx/plugins` directory
4. Launch the game

## Usage

1. Press `F6` to open/close the interpreter window
2. Type your BrainFuck code in the text field
3. Click "Execute Code [Result]" to see the final result
4. Click "Execute Code [Output]" to see the complete output
5. Drag the top bar of the window to reposition it

## Commands

BrainFuck uses the following eight commands:

| Command | Description |
| ------- | ----------- |
| `>` | Move the memory pointer to the right |
| `<` | Move the memory pointer to the left |
| `+` | Increment the value at the current memory cell |
| `-` | Decrement the value at the current memory cell |
| `.` | Output the character at the current cell |
| `,` | Input a character and store it in the current cell |
| `[` | Jump past the matching `]` if the current cell is 0 |
| `]` | Jump back to the matching `[` if the current cell is not 0 |

## Example

Hello World program:

```
++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.
```

## Credits

- Created by Steve (GUID: Steve.GorillaBrainFuck)
- Version: 1.0.0

```

This README provides a clear introduction to your mod, installation instructions, usage guide, and an example of BrainFuck code. Feel free to modify it as needed to better match your mod's specific features or your preferences!