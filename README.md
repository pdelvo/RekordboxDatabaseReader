# A .net rekordbox database reading library

A library that can be used to read the database format of rekordbox.

This is a really early version and still needs a lot of work. The main purpose of this was to play around with the new `Span<T>` and `Memory<T>` types.

The Internal namespace contain really low level readers that are (almost) allocation free (not that this really a big concern).

This is based on the data provided in 
[this](https://reverseengineering.stackexchange.com/questions/4311/help-reversing-a-edb-database-file-for-pioneers-rekordbox-software)
thread and [this](https://github.com/henrybetts/Rekordbox-Decoding) repository.

I also tried to work with the .edb format that the desktop rekordbox application uses but I could not find any information on it.
Some people claimed that it was the format used by the Extensible Storage Engine but when I looked at the file itself the formats do not seem to match.
