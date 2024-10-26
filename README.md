# SCPlus

 Plugin for [Plague Inc. Evolved](https://www.ndemiccreations.com/en/25-plague-inc-evolved)'s Scenario Creator that unlocks more functionality in the UI, intended for advanced use.
 Most patches are used in events: that is, to say, some general knowledge of events is required in order to truly leverage the mod.

 All features are compatible with (at least) the latest, unmodded build of Plague Inc. Evolved (PC). I cannot guarantee full support on mobile builds.

 If bugs which cannot be reproduced without the mod crop up, please do post them in [issues](https://github.com/9thCore/SCPlus/issues)!

# Feature list

- Patch translation to fix the infamous trait lowercasing bug!
- Patch the Disease Shape option so they're ordered like in the base game
- Expose more variables to the event outcome/condition editor
- Allow reading/writing of all variables, regardless of disease type
- Expose "Event lock" on traits, and locking/unlocking functionality on events
- Expose the Not requirements of traits (will become unavailable if selected traits are evolved), and remove the limit of 3 requirements per trait
- Allow creation of a convenient "Per Country" event type, which will run for every country if conditions match
	- Disabled by default, as it is not supported on mobile builds (they can only run one event a turn)
- Force *mutation* of one of selected traits in an event

 Features are categorised and toggleable in the config.
 For more details, see the [wiki](https://github.com/9thCore/SCPlus/wiki).

# Installation

1. [Install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html) (I use 5.4.21 myself, latest 5.4.X.X should work fine)
2. Obtain a copy of the compiled dll (whether this is through building the mod yourself, or through [Releases](https://github.com/9thCore/SCPlus/releases), your choice)
3. Drop plugin into Scenario Creator files: `PlagueInc/ScenarioCreator/BepInEx/plugins/SCPlus/the_mod_file.dll`
4. Run game
5. Check existence of `PlagueInc/ScenarioCreator/BepInEx/config/SCPlus.cfg`
6. If it exists, the mod has been correctly installed! This is the config through which you can choose what features to use