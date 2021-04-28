# [DEPRECATED]
## This repo was only for the first 4 weeks of development. The tests below are not guaranteed work anymore because it's incompatible with the server. The latest version of CosmicAPI is developed in https://github.com/LagomInteractive/cosmic-game

![Header image](https://raw.githubusercontent.com/LagomInteractive/CosmicAPI/master/Assets/Textures/banner.png)

# CosmicAPI for Unity

This API connects the game client to the cosmic game server.

### Realted repos
* Game - https://github.com/LagomInteractive/cosmic-game
* Server - https://github.com/LagomInteractive/cosmic-server

### Files
* [CosmicAPI.cs](https://github.com/LagomInteractive/CosmicAPI/blob/master/Assets/Scripts/CosmicAPI.cs) - This API
* [Tests.cs](https://github.com/LagomInteractive/CosmicAPI/blob/master/Assets/Scripts/Tests.cs) - Tests and examples for how to use CosmicAPI

If you clone this repo you can open the `CosmicAPI Test.unity` scene you can try it and see how it works.

### Dependencies

All dependencies are using upm and will automatically imported with a setup project.

* [NativeWebSocket](https://github.com/endel/NativeWebSocket.git#upm) - For the socket connection to the server
* [Newtonsoft](https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm) - For parsing and packing data over the sockets (JSON)

### Diagrams and Flowchart
* [Game overview](PDF/Overview.pdf)
* [CosmicAPI diagram](PDF/CosmicAPI.pdf)
* [CosmicAPI Events](PDF/Events.pdf)
