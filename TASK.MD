# Vertical Taskbar Support - Horizontal Separator Feature

## Problem
The current application is designed only for horizontal taskbar. When the taskbar is moved to the left or right side (vertical position), the separator continues to appear as a vertical line. In this case, the separator should appear as a horizontal bar.

## Goal
- Detect taskbar position (Left/Right/Top/Bottom)
- Use horizontal separator icon for vertical taskbar (Left/Right)
- Use existing vertical separator icon for horizontal taskbar (Top/Bottom)
- Create new horizontal separator.ico

## Technical Details

### 1. Taskbar Position Detection
Detect taskbar position using Windows API:
- Get taskbar info using `SHAppBarMessage` API
- Position detection with `ABM_GETTASKBARPOS` command
- Return `ABEdge` value: ABE_LEFT (0), ABE_TOP (1), ABE_RIGHT (2), ABE_BOTTOM (3)

### 2. Icon Management
- Existing: `separator.ico` (vertical bar)
- New: `separator_horizontal.ico` (horizontal bar)
- Load correct icon at runtime based on taskbar position

### 3. Form Dimensions
Adjust form dimensions based on taskbar position:
- **Vertical Taskbar** (Left/Right): Form width should be increased, height decreased
- **Horizontal Taskbar** (Top/Bottom): Keep existing dimensions

## Tasks

### Phase 1: Taskbar Position Detection ?
- [x] Create `TaskbarHelper.cs` class
- [x] Add Windows API interop definitions
- [x] Implement `GetTaskbarPosition()` method
- [x] Return taskbar position as enum

### Phase 2: Icon Creation ?
- [x] Convert existing `separator.ico` to horizontal version
- [x] Save as `separator_horizontal.ico`
- [x] Add to project file with CopyToOutputDirectory

### Phase 3: Form Updates ?
- [x] Add taskbar position check in `FormSplitter.cs`
- [x] Load correct icon based on position
- [x] Adjust form dimensions based on taskbar position
- [x] Added LoadHorizontalIcon() and LoadVerticalIcon() methods

### Phase 4: Testing ?
- [ ] Bottom taskbar (default) test
- [ ] Top taskbar test
- [ ] Left taskbar test
- [ ] Right taskbar test

## File Changes
```
Splitter/
??? TaskbarHelper.cs (NEW - ?)
??? FormSplitter.cs (UPDATED - ?)
??? Splitter.csproj (UPDATED - ?)
??? icons/
    ??? separator.ico (EXISTING)
    ??? separator_horizontal.ico (NEW - ?)
```

## Completed Steps
1. ? Created `TaskbarHelper.cs` with Windows API integration
2. ? Created `separator_horizontal.ico` from rotated original icon
3. ? Updated `FormSplitter.cs` to detect taskbar position and load appropriate icon
4. ? Updated `Splitter.csproj` to include new icon file
5. ? Build successful - no compilation errors

## Next Steps
1. Test application with taskbar in different positions
2. Verify icon rotation and appearance
3. Fine-tune form dimensions if needed

## API References
- [SHAppBarMessage](https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shappbarmessage)
- [APPBARDATA Structure](https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-appbardata)

## Notes
- When taskbar position is changed with ExplorerPatcher or similar tools, Windows API should return correct position
- Icon sizes should remain the same (16x16, 32x32, 48x48, 256x256)
- Transparency should be preserved
- Build completed successfully with no errors