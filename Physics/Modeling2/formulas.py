from math import *
from matplotlib import pyplot as plt


def calculate_x(t, C1, C2, C3, a):
    return C1 + C2 * cos(a * t) + C3 * sin(a * t)


def calculate_y(t, C2, C3, C4, a):
    return C4 + C3 * cos(a * t) - C2 * sin(a * t)


def make_graph(m, q, v, B, alpha, color: str):
    a = q * B / m

    C1 = (v * sin(alpha)) / a
    C2 = -1 * C1
    C3 = (v * cos(alpha)) / a
    C4 = -1 * C3

    arr_x = []
    arr_y = []
    arr_t = [i / 10**6 for i in range(10**6)]

    for num, t in enumerate(arr_t):
        arr_x.append(calculate_x(t, C1, C2, C3, a))
        arr_y.append(calculate_y(t, C2, C3, C4, a))
        if arr_y[-1] < 0:
            arr_t = arr_t[0:num]
            break

    plt.plot(arr_x, arr_y, color=color)
    plt.title("y(x)")
    # plt.ylim(0, 0.03)
    plt.show()
