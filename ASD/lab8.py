def count_ways(amount, coins):
    # Инициализация массива dp
    dp = [0] * (amount + 1)
    dp[0] = 1  # Базовый случай: одна комбинация для суммы 0

    for coin in coins:
        # Обновляем dp для всех сумм, начиная с текущего номинала
        for i in range(coin, amount + 1):
            dp[i] += dp[i - coin]

    return dp[amount]

if __name__ == "__main__":
    amount = 5
    coins = [1, 2, 5]
    print(f"Количество способов размена {amount}: {count_ways(amount, coins)}")
    # 4