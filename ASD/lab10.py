def find_critical_floor(N):
    step = 14  # начальный шаг
    current_floor = 0
    previous_floor = 0
    attempts = 0

    while step > 0 and current_floor < 100:
        attempts += 1
        previous_floor = current_floor
        current_floor += step
        if current_floor > 100:
            current_floor = 100

        # если яйцо разбилось
        if current_floor >= N:
            # проверяем все этажи между предыдущим и текущим
            for floor in range(previous_floor + 1, current_floor):
                attempts += 1
                if floor >= N:
                    return attempts
            return attempts

        step -= 1

    # если яйцо не разбилось ни разу, проверяем оставшиеся этажи
    for floor in range(current_floor + 1, 101):
        attempts += 1
        if floor >= N:
            return attempts

    return attempts

N = 45
print(f"Для этажа {N} нужно бросков: {find_critical_floor(N)}")
