# SCPlus

 Plugin for [Plague Inc. Evolved](https://www.ndemiccreations.com/en/25-plague-inc-evolved)'s Scenario Creator that unlocks more functionality in the UI, intended for advanced use.
 Most patches are used in events: that is, to say, some general knowledge of events is required in order to truly leverage the mod.

 All features are compatible with the latest, unmodded build of Plague Inc. Evolved (PC). Full support for console builds *is not guaranteed*, and mobile is *declared unsupported*.  
 Nothing stops a mobile client from downloading and playing the scenario, but using *anything* added in SCPlus is probably not going to work as expected. Simply having SCPlus installed without using any of its features, though, is perfectly fine.  
 (Tested on an unmodded, real mobile client)

 If bugs which cannot be reproduced without the mod crop up, please do post them in [the issue tracker](https://github.com/9thCore/SCPlus/issues)!

 #### Because the `win32beta` branch is built on an older Unity build, the `x86` mod version has to be used for it, and it will remove some features that are unavailable on this branch. The `x64` version *cannot* be used on the `win32beta` branch, but the `x86` version can be used on the stable branch. (Though the features will *remain locked*, so there is no point in doing so.)

# Feature list
Some features may not be available on the `x86` build of the mod. They will be marked by `(x64)`. Everything else can be safely assumed to work as expected.

- Patch translation to fix the infamous trait lowercasing bug!
- `(x64)` Patch the Disease Shape option so they're ordered like in the base game
- Expose more variables to the event outcome/condition editor
- Allow reading/writing of all variables, regardless of disease type
- `(x64)` Expose "Event lock" on traits, and locking/unlocking functionality on events
- Expose the Not requirements of traits (will become unavailable if selected traits are evolved), and remove the limit of 3 requirements per trait
- Allow creation of a convenient "Per Country" event type, which will run for every country if conditions match
	- Disabled by default
- Force *mutation* of one of selected traits in an event

 Features are categorised and toggleable in the config.
 For more details, see the [wiki](https://github.com/9thCore/SCPlus/wiki).

# Installation
Some steps may vary between the `x86` and `x64` mod builds. They will be marked by their version.

- [Install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html) (Any 5.4.* should work just fine, even latest)
- `(x64)` Run the Scenario Creator, either from in-game or from the files (`PlagueIncSC.exe`)
- `(x86)` Run `PlagueIncSC.exe` from the files
- Check existence of `PlagueInc/ScenarioCreator/BepInEx`
- Obtain a copy of the compiled dll (whether this is through building the mod yourself, or through [Releases](https://github.com/9thCore/SCPlus/releases), your choice)
- `(x86)` Download the `x86` mod build, or build on the `Release32` configuration.
- `(x64)` Download the `x64` mod build, or build on the `Release` configuration.
- Drop plugin into Scenario Creator files: `PlagueInc/ScenarioCreator/BepInEx/plugins/SCPlus/the_mod_file.dll`
- `(x64)` Run the Scenario Creator, either from in-game or from the files (`PlagueIncSC.exe`)
- `(x86)` Run `PlagueIncSC.exe` from the files
- Check existence of `PlagueInc/ScenarioCreator/BepInEx/config/SCPlus.cfg`
- If it exists, the mod has been correctly installed! This is the config through which you can choose what features to use
