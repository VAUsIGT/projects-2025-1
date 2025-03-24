# таблица плохих символов для алгоритма
def build_bad_char_table(pattern):
    table = {}
    m = len(pattern)
    for i in range(m):
        table[pattern[i]] = i  # сохраняем последнее вхождение каждого символа
    return table

# алгоритм Бойера-Мура
def boyer_moore(text, pattern):
    bad_char_table = build_bad_char_table(pattern)
    m = len(pattern)
    n = len(text)
    results = []
    i = 0  # текущая позиция в тексте

    while i <= n - m:
        j = m - 1  # индекс в образце, начинаем с конца

        # сравниваем символы справа налево
        while j >= 0 and pattern[j] == text[i + j]:
            j -= 1

        if j == -1:
            # весь образец совпал
            results.append(i)
            # сдвигаемся, учитывая символ после образца
            if i + m < n:
                c = text[i + m]
                shift = m - bad_char_table.get(c, -1)
            else:
                shift = 1  # если за образцом ничего нет
            shift = max(shift, 1)
            i += shift
        else:
            # несовпадение на позиции j
            c = text[i + j]
            last_occurrence = bad_char_table.get(c, -1)
            # вычисляем сдвиг для плохого символа
            shift = max(1, j - last_occurrence)
            i += shift

    return results