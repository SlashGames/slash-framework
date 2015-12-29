# Slash Framework

[![Coverage Status](https://coveralls.io/repos/npruehs/slash-framework/badge.svg?branch=develop&service=github)](https://coveralls.io/github/npruehs/slash-framework?branch=develop)

The Slash Framework is a free, open-source, component-based entity system (CBES) framework especially for games.

The design and rules of games constantly change during development, invalidating your carefully engineered software from day to day. 

Entity systems are a great approach for getting rid of the many drawbacks of inheritance-based game models like the “diamond of death”, moving on to a much more flexible aggregation-based model which has been popular since Gas Powered Games’ Dungeon Siege from 2003.

The Slash Framework provides both a low-level implementation of component-based entity systems and Unity3D integration for them.

If you're missing anything, we'd love to see it - please refer to the [Contributing](https://github.com/npruehs/slash-framework/blob/master/CONTRIBUTING.md) file!

## Features

* Low-level implementation of component-based entity systems
* Unity3D integration
* Proven to work in several released games (see Show Cases)
* Unit tests and references for non-trivial operations
* Several articles about specifics and usage of the framework
* [Public wiki](http://slashgames.org:8090/display/SF)
* [Public API documentation](http://www.slashgames.org/content/framework/api/1.0)

## Getting Slash Framework

You can either

* download BINARIES from the [Releases](https://github.com/npruehs/slash-framework/releases) page (coming soon)
* checkout SOURCES from this repository

## Getting started

If you are unfamiliar with component-based entity systems in general, you should read a few general articles first:

* http://www.gamedev.net/page/resources/_/technical/game-programming/understanding-component-entity-systems-r3013
* http://www.gamedev.net/page/resources/_/technical/game-programming/case-study-bomberman-mechanics-in-an-entity-component-system-r3159
* http://cowboyprogramming.com/2007/01/05/evolve-your-heirachy/
* http://t-machine.org/index.php/2007/09/03/entity-systems-are-the-future-of-mmog-development-part-1/
* http://alecmce.com/blog/why-use-entity-systems-for-game-engineering
* http://www.richardlord.net/blog/what-is-an-entity-framework
* http://www.gamedev.net/page/resources/_/technical/game-programming/entities-parts-i-game-objects-r3596
* https://jlnr.de/2011/01/04/game-object-inheritance.html

Introductions, specifics and tutorials about our framework can be found in several articles we wrote over the years:

* http://www.npruehs.de/game-models-a-different-approach-i/
* http://www.npruehs.de/game-models-a-different-approach-part-2/
* http://www.npruehs.de/one-does-not-simply/
* http://unity-coding.slashgames.org/component-based-entity-systems-project-setup/
* http://unity-coding.slashgames.org/cbes-entity-manager-and-co/
* http://unity-coding.slashgames.org/component-based-entity-systems-event-driven-systems-to-implement-the-logic/
* http://unity-coding.slashgames.org/cbes-creating-a-component-based-game/
* http://unity-coding.slashgames.org/why-to-use-a-custom-cbes-architecture-within-unity/
* http://unity-coding.slashgames.org/events-order-matters/

To get more information about the framework, take a glimpse at the [API documentation](http://www.slashgames.org/content/framework/api/1.0) and [official wiki](http://slashgames.org:8090/display/SF).

## Remarks

For each of our non-trivial implementations, we provide unit tests and references to algorithms. If anything's unclear, feel free to contact us. We'd love to improve our [documentation](http://slashgames.org:8090/display/SF).

## Development Cycle

We know that using a framework in production requires you to be completely sure about stability and compatibility. Thus, new releases of the Slash Framework are created using [Semantic Versioning](http://semver.org/). In short:

* Version numbers are specified as MAJOR.MINOR.PATCH.
* MAJOR version increases indicate incompatible changes with respect to the public Slash Framework API.
* MINOR version increases indicate new functionality that are backwards-compatible.
* PATCH version increases indicate backwards-compatible bug fixes.

## Bugs & Feature Requests

We are sorry that you've experienced issues or miss a feature! After verifying that you are using the latest version of Slash Framework and having checked whether a [similar issue](https://github.com/npruehs/slash-framework/issues) has already been reported, feel free to [open a new issue](https://github.com/npruehs/slash-framework/issues/new). In order to help us resolving your problem as fast as possible, please include the following details in your report:

* Steps to reproduce
* What happened?
* What did you expect to happen?

After being able to reproduce the issue, we'll look into fixing it immediately.

## Show Cases

We used our framework in nearly all of our past projects, including:

* FreudBot ([Android](https://play.google.com/store/apps/details?id=org.slashgames.FreudBot.AdFree), [iOS](https://itunes.apple.com/us/app/freudbot/id930042591), [Windows](https://www.microsoft.com/en-us/store/games/freudbot/9wzdncrdfr7p))
* [ToyHunt](http://www.slashgames.org/toyhunt/)
* [Doom Dino](http://www.slashgames.org/doom-dino/)
* [HeroCast](http://www.slashgames.org/herocast/)
* [Robot Meltdown](http://www.slashgames.org/robot-meltdown/)

If you used the Slash Framework in one of your projects, we are very happy to hear about it and add it to the list of show cases!

## Contributors

(in no particular order)

* [Nick Prühs](https://github.com/npruehs)
* [Christian Oeing](https://github.com/coeing)

## License

Slash Framework is released under the [MIT license](https://github.com/npruehs/slash-framework/blob/master/LICENSE).
