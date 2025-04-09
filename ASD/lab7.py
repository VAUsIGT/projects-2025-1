def max_subarray(nums):
    if not nums:
        return (0, [], 0, 0)  # обработка пустого ввода

    max_current = max_global = nums[0]
    start_current = 0
    start_global = 0
    end_global = 0

    for i in range(1, len(nums)):
        num = nums[i]
        if num > max_current + num:
            max_current = num
            start_current = i
        else:
            max_current += num
        if max_current > max_global:
            max_global = max_current
            start_global = start_current
            end_global = i
    subarray = nums[start_global: end_global + 1]
    return (max_global, subarray, start_global, end_global)

nums = [-2, 1, -3, 4, -1, 2, 1, -5, 4]
max_sum, subarray, start, end = max_subarray(nums)
print(f"Максимальная сумма: {max_sum}")
print(f"Подмассив: {subarray}")
print(f"Индексы: с {start} по {end}")