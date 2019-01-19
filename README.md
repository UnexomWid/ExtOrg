# About

ExtOrg is an application written in C# that organizes files based on their extensions

![EX](EX.PNG)

# License

ExtOrg is a project by [UnexomWid](http://unexomwid.github.io). The latest version is *1.1*. It is licensed under the [General Public License (GPL) version 3](https://www.gnu.org/licenses/gpl-3.0.en.html).

# Usage

ExtOrg moves all files from the working directory to folders named after their extensions. It does not affect files in the subfolders. Example:
>D:/folder/file.txt -> D:/folder/txt/file.txt

If a file already exists in the destination folder, the application will prompt you to decide whether or not to overwrite that file. You can also choose to overwrite/skip these files in the future.

# Options

*For more details, run **extorg.exe --help**.

By default, ExtOrg uses the current directory as the working one. However, you can specify the working directory. Example:
>extorg.exe "D:/path/to/folder"

You can also specify how the application should handle *file-already-exists* conflicts. Example:
>extorg.exe -y // automatically overwrites existing files
>extorg.exe -n // automatically skips existing files

You can combine these two options however you want.