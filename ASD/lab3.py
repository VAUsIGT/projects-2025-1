# создаёт массив pi, где pi[q] длина наибольшего префикса подстроки
def compute_prefix_function(pattern):
    m = len(pattern)
    pi = [0] * m
    k = 0
    for q in range(1, m):
        while k > 0 and pattern[q] != pattern[k]:
            k = pi[k - 1]
        if pattern[q] == pattern[k]:
            k += 1
        pi[q] = k
    return pi

# таблица переходов, для каждого состояния q и символа c определяется след. сост.
# Если c совпадает с текущ. симв. образца - переходит на след. сост. Иначе берёт значение префикс функции для отката
def build_transition_table(pattern):
    m = len(pattern)
    if m == 0:
        return []
    alphabet = set(pattern)
    pi = compute_prefix_function(pattern)
    transition = [{} for _ in range(m + 1)]

    for q in range(m + 1):
        for c in alphabet:
            if q < m and c == pattern[q]:
                transition[q][c] = q + 1
            else:
                if q == 0:
                    transition[q][c] = 0
                else:
                    k = pi[q - 1]
                    transition[q][c] = transition[k].get(c, 0)
    return transition

# проходит по тексту, при состоянии, равном длине образца фиксирует начало вхождения
def finite_automaton_search(text, pattern):
    m = len(pattern)
    if m == 0:
        return []
    transition = build_transition_table(pattern)
    current_state = 0
    occurrences = []
    for i, c in enumerate(text):
        current_state = transition[current_state].get(c, 0)
        if current_state == m:
            start_index = i - m + 1
            occurrences.append(start_index)
    return occurrences


# сложность O(n) - n длина текста
if __name__ == "__main__":
    text = "ABABABAC"
    pattern = "ABABAC"
    print(finite_automaton_search(text, pattern))