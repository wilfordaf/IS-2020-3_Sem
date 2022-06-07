from math import *
from matplotlib import pyplot as plt


def task1():
    n_red = 1.331
    phi1_red = []
    phi2_red = []

    n_purple = 1.343
    phi1_purple = []
    phi2_purple = []

    y_positive = [i / 100 for i in range(0, 100, 1)]
    y_negative = [i / 100 for i in range(-99, 0, 1)]

    for y in y_positive:
        alpha1 = asin(y)
        alpha2_red = asin(y / n_red)
        alpha2_purple = asin(y / n_purple)

        phi1_red.append(round(degrees(4 * alpha2_red - 2 * alpha1), 2))
        phi1_purple.append(round(degrees(4 * alpha2_purple - 2 * alpha1), 2))

    for y in y_negative:
        alpha1 = asin(-1 * y)
        alpha2_red = asin(-1 * y / n_red)
        alpha2_purple = asin(-1 * y / n_purple)

        phi2_red.append(round(degrees(pi - 6 * alpha2_red + 2 * alpha1), 2))
        phi2_purple.append(round(degrees(pi - 6 * alpha2_purple + 2 * alpha1), 2))

    print(f"Maximum φ1 for red light is {max(phi1_red)}")
    print(f"Minimum φ2 for red light is {min(phi2_red)}")
    print(f"Maximum φ1 for purple light is {max(phi1_purple)}")
    print(f"Minimum φ2 for purple light is {min(phi2_purple)}")

    plt.figure(figsize=(12, 8))
    plt.subplot(1, 2, 1)
    plt.title("φ1(y)")
    plt.plot(y_positive, phi1_red, color="red")
    plt.plot(y_positive, phi1_purple, color="purple")
    plt.subplot(1, 2, 2)
    plt.title("φ2(y)")
    plt.plot(y_negative, phi2_red, color="red")
    plt.plot(y_negative, phi2_purple, color="purple")
    plt.show()


def task2():
    n_red = 1.331
    n_purple = 1.346
    arr_y = [i / 10000 for i in range(1, 10000)]
    phi_red = [round(degrees(4 * asin(arr_y[0] / n_red) - 2 * asin(arr_y[0])), 2)]
    phi_purple = [round(degrees(4 * asin(arr_y[0] / n_purple) - 2 * asin(arr_y[0])), 2)]
    der_phi_red = [1 / (4 / (sqrt(n_red ** 2 - arr_y[0] ** 2)) - 2 / sqrt(1 - arr_y[0] ** 2))]
    der_phi_purple = [1 / (4 / (sqrt(n_purple ** 2 - arr_y[0] ** 2)) - 2 / sqrt(1 - arr_y[0] ** 2))]

    for number, y in enumerate(arr_y):
        if number == 0:
            continue

        der_phi_red_val = 1 / (4 / (sqrt(n_red ** 2 - y ** 2)) - 2 / sqrt(1 - y ** 2))
        der_phi_purple_val = 1 / (4 / (sqrt(n_purple ** 2 - y ** 2)) - 2 / sqrt(1 - y ** 2))

        if der_phi_red_val >= der_phi_red[-1]:
            phi_red.append(round(degrees(4 * asin(y / n_red) - 2 * asin(y)), 2))
            der_phi_red.append(1 / (4 / (sqrt(n_red ** 2 - y ** 2)) - 2 / sqrt(1 - y ** 2)))

        if der_phi_purple_val >= der_phi_purple[-1]:
            phi_purple.append(round(degrees(4 * asin(y / n_purple) - 2 * asin(y)), 2))
            der_phi_purple.append(1 / (4 / (sqrt(n_purple ** 2 - y ** 2)) - 2 / sqrt(1 - y ** 2)))

    plt.figure(figsize=(8, 8))
    plt.title("I(φ)")
    plt.plot(phi_red, der_phi_red, color="red")
    plt.plot(phi_purple, der_phi_purple, color="purple")
    plt.show()


task1()
task2()

