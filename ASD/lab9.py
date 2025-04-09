import sys

def tsp(distance):
    n = len(distance)
    INF = float('inf')

    # dp[mask][i] = минимальная стоимость пройти города в mask и оказаться в городе i
    dp = [[INF] * n for _ in range(1 << n)]
    dp[1][0] = 0  # начинаем с города 0, только он посещён (mask = 1)

    for mask in range(1 << n):
        for u in range(n):
            if not (mask & (1 << u)):  # если u не в маске
                continue
            for v in range(n):
                if mask & (1 << v):  # если v уже в маске
                    continue
                next_mask = mask | (1 << v)
                dp[next_mask][v] = min(dp[next_mask][v], dp[mask][u] + distance[u][v])

    # Финальный шаг — возвращение в город 0
    min_cost = INF
    for i in range(1, n):
        if dp[(1 << n) - 1][i] + distance[i][0] < min_cost:
            min_cost = dp[(1 << n) - 1][i] + distance[i][0]

    return min_cost
