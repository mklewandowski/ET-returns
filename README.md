# ET-returns
E.T. Returns is a sorta-sequel, sorta-remix of the iconic Atari 2600 game that is sort of a classic and maybe ushered in the downfall of the entire home videogame industry*. Heavily influenced by by [Vampire Surivors](https://poncle.itch.io/vampire-survivors) and borrowing characters from dozens of retro games and Atari 2600 titles, E.T. Returns features copious helpings of homage and action parody. Upgrade your E.T., unlock characters, and shoot holes in the heroes of yesterday.

*Alex Smith's recent research digs into the real reasons behind the 1982/1983 videogame crash. You can hear more on his excellent podcast, [They Create Worlds](https://www.theycreateworlds.com/).

## Gameplay
- Destroy earth heroes and collect candy to level up.
- Destroy FBI agents and collect phone pieces.
- Collect 3 phone pieces and phone in an upgrade.
- Survive as long as you can.

![E.T. Returns gameplay](https://github.com/mklewandowski/et-returns/blob/main/Assets/Images/et-returns-gameplay.gif?raw=true)

### Controls
Use arrow keys, WASD keys, or a controller to move.

## Supported Platforms
E.T. Returns is designed for use on multiple platforms including:
- Web
- Mac and PC standalone builds

## Running Locally
Use the following steps to run locally:
1. Clone this repo
2. Open repo folder using Unity 2021.3.23f1
3. Install Text Mesh Pro

## Building the Project

### WebGL Build
For embedding within itch.io and other web pages, we use the `better-minimal-webgl-template` seen here:
https://seansleblanc.itch.io/better-minimal-webgl-template

Setup of the `better-minimal-webgl-template` is as follows:
1. Download and unzip the template.
2. Copy the `WebGLTemplates` folder into the `Assets` folder.
3. File -> Build Settings... -> WebGL -> Player Settings... -> Select the "BetterMinimal" template.
4. Enter color in the "Background" field.
5. Enter "false" in the "Scale to fit" field to disable scaling.
6. Enter "true" in the "Optimize for pixel art" field to use CSS more appropriate for pixel art.

### Running a Unity WebGL Build
1. Install the "Live Server" VS Code extension.
2. Open the WebGL build output directory with VS Code.
3. Right-click `index.html`, and select "Open with Live Server".

## Development Tools
- Created using Unity
- Code edited using Visual Studio Code
- Sounds created using [Bfxr](https://www.bfxr.net/)
- Audio edited using [Audacity](https://www.audacityteam.org/)
- 2D images created and edited using [Paint.NET](https://www.getpaint.net/)

## Credits
Pixel art ripped from original Atari VCS games.