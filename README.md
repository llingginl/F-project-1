# F-project-1
Group mumeber:

Yizhe Wu, UFID: 4491-6383;
Yepeng Liu, UFID: 1980-9506;
1. Source Files Introduction:
Implementing actor medol locally: local.fsx;
Implementing actor medol remotelly: remote.fsx, server.fsx;

2. Usage:
If working locally:

cd to DOSP_proj_1 folder;
Run command dotnet fsi test.fsx m in CLI, where m is the number of zero you are going to find;
If working remotelly:

Two computer cd to DOSP_proj_1 folder respectively;
Run command dotnet fsi remote.fsx in CLI on first PC;
Run command dotnet fsi server.fsx m in CLI on second PC, where m is the number of zero you are going to find;
Results will be shown in CLI on second PC;
3. Project Details:
(1). Size of the work unit:
We havee 62 actors to work in parallel. For distributing the work load on average, we give every actors different and fixed first character of the suffix.

(2). The result of running your program for input 4:
liuyepeng@liuyepengdeMacBook-Pro-3 Downloads % dotnet fsi test.fsx 4
Real: 00:00:00.000，CPU: 00:00:00.000，GC gen0: 0, gen1: 0, gen2: 0
[("wuyizhe;liuyepeng;VGg", "0000cc228ad772d259010892b926d8cb721b5169211176ca666f646fb9416fa7")]
[("wuyizhe;liuyepeng;4cO", "0000d966ca985e4e29c5fab1a2376988fe7343188c3b6a90899802ace19b4a7e")]
Real: 00:00:06.665，CPU: 00:00:11.921，GC gen0: 641, gen1: 159, gen2: 3

(3). The ratio of CPU time to REAL TIME:
The ratio of CPU time to REAL TIME is about 1.79.

(4). The coin with the most 0s you managed to find:
We managed to find five 0s, using 4-core intel core i7. We can also find more 0s, but it will take more time.

(5). The largest number of working machines you were able to run your code with:
We tested two working machines to run the code, and it works. In our program, we can also create more working machines to distribute the work.
