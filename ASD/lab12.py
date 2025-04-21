def knapsack(weights, values, capacity): # веса, стоимости, вместимость, возвращает кортеж (макс стоим, индекс выбранных)
    n = len(weights)
    # dp[w] = макс стоимость при вместимости ровно w
    dp = [0] * (capacity + 1)

    # для каждого предмета i «перебираем» возможные, оставшиеся вместимости w от capacity вниз до weights[i]
    for i in range(n):
        wi, vi = weights[i], values[i]
        for w in range(capacity, wi - 1, -1):
            # либо не берём i‑й предмет (dp[w]), либо берём (dp[w‑wi] + vi)
            dp[w] = max(dp[w], dp[w - wi] + vi)

    # восстанавливаем список выбранных предметов
    selected = []
    w = capacity
    # идём с конца списка предметов к началу
    for i in range(n - 1, -1, -1):
        wi, vi = weights[i], values[i]
        # если при вместимости w мы получили именно добавлением этого предмета
        if w >= wi and dp[w] == dp[w - wi] + vi:
            selected.append(i)
            w -= wi
    selected.reverse()

    return dp[capacity], selected

if __name__ == "__main__":
    weights = [1, 3, 4, 5]
    values  = [1, 4, 5, 7]
    capacity = 7

    max_value, items = knapsack(weights, values, capacity)
    print("Максимальная стоимость:", max_value)
    print("Выбранные предметы (индексы):", items)
