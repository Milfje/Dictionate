# Dictionate
Dictionate is a simple password dictionary creation tool for Windows. It accepts a source file of lines (words, numbers, characters) to produce combinations of to an output file.

# Usage

Clone the repository and build the tool, or download one of the [pre-built releases](https://github.com/Milfje/Dictionate/releases).

### Default

By default, `Dictionate.exe` expects a source file with the `-s` parameter. In the default mode, the depth (number of words to concatenate to the source list max) is set to 2 and repetition is set to false. This means the words in your source list won't by combined with themselves.

```
Dictionary.exe -s input.txt
```

### Commandline arguments

The additional options for `Dictionate.exe` can be found by using the `--help` flag, or by running `Dictionate.exe` without specifying a required input file. The options are:

 Short | Extended | Required | Default | Description 
 :---: | --- | --- | --- | ---
 -s | --source | yes | - | The source path/file of words, numbers and characters (per line) to make combinations between.
 -o | --output | no | `output.txt` |The output path/file with combinations. By default this will be `output.txt`.
 -d | --depth | no | 2 | The maximum amount of combinations added to the original words in the source list. By default this will be 2.
 -c | --capitalize | no | - | This flag will append the source list with capitalized versions of the source words <br /> (for example `dog > Dog`).
 -a | --all-caps | no | - | Will append the source list with full capitalization of the source words <br /> (for example `dog > DOG`).
 -r | --repetition | no | `false` | This flag will tell Dictionate to allow repetition. Depending on the depth words from the source list will be combined with themselves.
 -x | --silent | no | `false` | This will disable the verbose output of the program.
