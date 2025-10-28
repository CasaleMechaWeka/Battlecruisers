# Runtime Console Logging

## The Problem
Unity's Editor.log only captures Editor infrastructure logs, NOT runtime logs (Debug.Log, exceptions, etc.) during Play mode. This means critical errors visible in Unity's Console window are NOT written to disk, making them hard to debug.

## The Solution
`ConsoleToFileCapture.cs` hooks into Unity's logging system and writes ALL console output to:
```
RuntimeConsole.log
```
(Project root, next to Assets folder)

## Setup
The script is already in place at:
```
Assets/Scripts/Utils/ConsoleToFileCapture.cs
```

**It starts automatically when you enter Play mode.** No configuration needed.

## Usage

### View the runtime log:
```bash
# View entire log
cat RuntimeConsole.log

# View last 100 lines
tail -100 RuntimeConsole.log

# Grep for specific errors
cat RuntimeConsole.log | grep -i "LobbyServiceException"

# Search with context
grep -A 30 "LobbyServiceException" RuntimeConsole.log
```

### What it captures:
- ✅ Debug.Log()
- ✅ Debug.LogWarning()
- ✅ Debug.LogError()
- ✅ Exceptions (including full stack traces)
- ✅ All your "privatepvp:" logs
- ✅ Unity SDK exceptions (Lobby, Netcode, etc.)

### Log format:
```
HH:mm:ss.fff [TYPE] Message
Stack trace (if error/exception)

HH:mm:ss.fff [TYPE] Next message...
```

## Debugging Workflow
When debugging runtime issues:
1. First check `RuntimeConsole.log` for runtime errors
2. Then check `Editor.log` for compiler/editor infrastructure issues

**Quick error check:**
```bash
cat RuntimeConsole.log | grep -iA 30 "exception\|error"
```
