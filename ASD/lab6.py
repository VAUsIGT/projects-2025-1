# поиск подстроки (паттерн) в тексте по Рабина-Карпа, prime простое число для уменьшения коллизий
def rabin_karp(text, pattern, prime=101):
    n = len(text)
    m = len(pattern)
    base = 256  # колво символов в алфавите (обычно 256 для аски)
    pattern_hash = 0  # хеш образца
    text_hash = 0  # хеш текущего окна в тексте
    h = 1  # вспомогательное число для вычисления хеша
    result = []
    # вычисляем (base^(m-1)) % prime
    for i in range(m - 1):
        h = (h * base) % prime
    # вычисляем начальные хеши для образца и первой подстроки текста
    for i in range(m):
        pattern_hash = (base * pattern_hash + ord(pattern[i])) % prime
        text_hash = (base * text_hash + ord(text[i])) % prime
    # сканируем текст
    for i in range(n - m + 1):
        # если хеши совпали, проверяем символы
        if pattern_hash == text_hash:
            if text[i:i + m] == pattern:  # дополнительная проверка во избежание ложных срабатываний
                result.append(i)
        # вычисляем хеш следующего окна в тексте (если оно есть)
        if i < n - m:
            text_hash = (text_hash - ord(text[i]) * h) * base + ord(text[i + m])
            text_hash %= prime  # чтобы избежать переполнения
            # python может давать отрицательные хеши, исправление
            if text_hash < 0:
                text_hash += prime
    return result

text = "ABCDEABCABCDE"
pattern = "ABC"
print(rabin_karp(text, pattern))  # [0, 5, 8]
