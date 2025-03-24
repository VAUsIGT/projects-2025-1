# вычисляет массив длин наиб префиксов, которые также и суффиксы (LPS), лпс помогает определить, на сколько символов сдвинуть образец при несовпадении
def compute_lps(pattern):
    lps = [0] * len(pattern)
    i = 0  # индекс для отслеживания длины текущего префикса
    for j in range(1, len(pattern)):
        # пока есть несовпадение, возвращаемся к предыдущему префиксу
        while i > 0 and pattern[j] != pattern[i]:
            i = lps[i - 1]
        # если символы совпадают, увеличиваем длину текущего префикса
        if pattern[j] == pattern[i]:
            i += 1
            lps[j] = i
        else:
            lps[j] = 0  # нет совпадения, префикс нулевой длины
    return lps

# алгоритм кмп для поиска всех вхождений в тексте, возвращает список индексов начала совпадений
def kmp_search(text, pattern):
    if not pattern:
        return []
    lps = compute_lps(pattern)
    i = j = 0  # индексы для текста (i) и образца (j)
    matches = []
    while i < len(text):
        if text[i] == pattern[j]:
            # совпадение символов, двигаемся дальше
            i += 1
            j += 1
            if j == len(pattern):
                # найдено полное совпадение
                matches.append(i - j)
                j = lps[j - 1]  # сдвигаем образец по LPS
        else:
            if j != 0:
                # сдвигаем образец по LPS, текст не двигаем
                j = lps[j - 1]
            else:
                # нет совпадения и j=0, двигаем только текст
                i += 1
    return matches

text = "ABABABAC"
pattern = "ABABAC"
print(kmp_search(text, pattern))  # 2