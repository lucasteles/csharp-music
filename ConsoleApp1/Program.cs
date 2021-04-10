using System;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.IO;

// Console.Beep(52,2 ); // only windows

var volume = .5f;

var step = .05f;
var wave = Enumerable
    .Range(0, 48000)
    .Select(x => x * step)
    .Select(x => (float)Math.Sin(x))
    .Select(x => x * volume);

var bytes = wave.SelectMany(BitConverter.GetBytes).ToArray();
var filename = "./output.bin";



if (File.Exists(filename)) File.Delete(filename);
await File.WriteAllBytesAsync(filename,bytes);
Process.Start("ffplay"," -f f32le -ar 48000 ./output.bin");