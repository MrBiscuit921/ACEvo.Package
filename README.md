# ACEvo.Package

KSPackage archive tool for Assetto Corsa EVO.

NOTE: **Absolutely no support will be provided.** I don't normally deal with racing games at all anymore but this didn't take more than 2 hours.

## Side Notes

The majority of the game content/files uses [protobuf](https://protobuf.dev/) schemas which may need to be reversed.

## Modding

It is supposedly possible to dump the game contents to the root of the game and renaming `content.kspkg` so that the game doesn't read it, so that the game can run unpacked.

## Usage

* Extract all files: `ACEvo.Package.CLI.exe unpack <path_to_kspkg> <output_directory>`
* Extract specifiec file: `ACEvo.Package.CLI.exe unpack-file <path_to_kspkg> <game_path> <output_directory>`
* List all files in a package: `ACEvo.Package.CLI.exe list-files <path_to_kspkg>`

## Building

.NET 9.0 and Visual Studio 2022.

Contributions are welcome.

## License

MIT License.

## Other Projects

* [kspkg-viewer](https://github.com/sa413x/kspkg-viewer) (C++)
* [ace-kspkg](https://github.com/ntpopgetdope/ace-kspkg) (Python)
