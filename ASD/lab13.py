def first_fit_decreasing(sizes, bin_capacity): # размеры предметов, вместимость одного ящика, возвращает список ящиков
    # Сортируем по убыванию, храним исходные индексы
    indexed = sorted(enumerate(sizes), key=lambda x: x[1], reverse=True)
    bins = []            # в каждом — список индексов
    rem_caps = []        # соответствующие остатки вместимости

    for idx, sz in indexed:
        placed = False
        for i, rem in enumerate(rem_caps):
            if sz <= rem:
                bins[i].append(idx)
                rem_caps[i] -= sz
                placed = True
                break
        if not placed:
            bins.append([idx])
            rem_caps.append(bin_capacity - sz)
    return bins


def optimal_bin_packing(sizes, bin_capacity): # список размеров, вместимость одного, возвращает список ящиков
    n = len(sizes)
    # Сортируем по убыванию, чтобы сильнее отсекать
    order = sorted(range(n), key=lambda i: sizes[i], reverse=True)
    # Начальное “лучшее” решение — из FFD
    best_bins = first_fit_decreasing(sizes, bin_capacity)
    best_count = len(best_bins)
    solution = [b.copy() for b in best_bins]

    curr_bins = []
    curr_rem = []

    def dfs(pos):
        nonlocal best_count, solution
        # 1) Если все позиции обработаны — нашли полную укладку
        if pos == n:
            if len(curr_bins) < best_count:
                best_count = len(curr_bins)
                solution = [b.copy() for b in curr_bins]
            return
        # 2) Отсечка: уже не лучше текущего лучшего
        if len(curr_bins) >= best_count:
            return

        i = order[pos]
        sz = sizes[i]

        seen_rem = set()
        # Попытаться положить в каждый существующий ящик
        for b, rem in enumerate(curr_rem):
            if rem >= sz and rem not in seen_rem:
                seen_rem.add(rem)
                curr_bins[b].append(i)
                curr_rem[b] -= sz
                dfs(pos + 1)
                curr_rem[b] += sz
                curr_bins[b].pop()

        # Или открыть новый ящик
        curr_bins.append([i])
        curr_rem.append(bin_capacity - sz)
        dfs(pos + 1)
        curr_bins.pop()
        curr_rem.pop()

    dfs(0)
    return solution


if __name__ == "__main__":
    sizes = [4, 8, 1, 4, 2, 1, 7, 3]
    C = 10

    print("First-Fit Decreasing") # быстрый nlogn, но не самый эффективный
    ffd = first_fit_decreasing(sizes, C)
    print(f"Ящиков: {len(ffd)}")
    for i, b in enumerate(ffd, 1):
        print(f" Ящик {i}: items {b}, sum = {sum(sizes[j] for j in b)}")

    print("\nBranch-and-Bound") # эффективный, но экспоненциально медленный, лучше для n<40
    opt = optimal_bin_packing(sizes, C)
    print(f"Ящиков: {len(opt)}")
    for i, b in enumerate(opt, 1):
        print(f" Ящик {i}: items {b}, sum = {sum(sizes[j] for j in b)}")
