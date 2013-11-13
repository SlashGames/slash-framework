Small tool from https://github.com/jaredthirsk/AOT-Compatlyzer/ to make DLLs AOT compatible.

This is required due to DLLs build with VS2010 or later not being AOT compatible as Unity3D uses old Mono versions. The issue occurs for events in the DLLs: An EngineException is thrown each time you register a delegate on one of those events.

See thread http://forum.unity3d.com/threads/113750-ExecutionEngineException-on-iOS-only for details.