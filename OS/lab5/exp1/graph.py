import re
import matplotlib.pyplot as plt


def timeConvert(time):
    return int(time[0]) * 3600 + int(time[1]) * 60 + int(time[2])


with open("parameters.log", "r") as fin:
    time = []
    freeMem = []
    freeSwap = []
    starttime = re.sub('\n', '', fin.readline())
    starttime = starttime.split(sep=":")
    starttime = timeConvert(starttime)
    time.append(0)
    for line in fin.readlines():
        line = re.sub('\n', '', line)
        if len(line) == 8:
            line = line.split(sep=":")
            time.append(timeConvert(line) - starttime)
        elif "MiB Mem" in line:
            line = line.split(sep=",")
            line = re.sub('[ fre]', '', line[1])
            freeMem.append(float(line))
        elif "MiB Swap" in line:
            line = line.split(sep=",")
            line = re.sub('[ fre]', '', line[1])
            freeSwap.append(float(line))
    time = time[0:124]
    freeMem = freeMem[0:124]
    freeSwap = freeSwap[0:124]
    # print(time)
    # print(freeMem)
    plt.xlabel("Time from the start of the experiment, sec")
    plt.ylabel("Free memory, Mb")
    plt.plot(time, freeMem, color="blue", label="Free memory")
    plt.plot(time, freeSwap, color="orange", label="Free swap")
    plt.legend()
    plt.show()
