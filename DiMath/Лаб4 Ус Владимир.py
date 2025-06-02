import re
from collections import defaultdict, Counter
import heapq
import math


# 1. Подготовка текста (~8000 символов, ≤64 уникальных)
def generate_text():
    """Генерация текста на основе 'Гамлета' Шекспира"""
    hamlet_fragment = """
    To be, or not to be: that is the question:
    Whether 'tis nobler in the mind to suffer
    The slings and arrows of outrageous fortune,
    Or to take arms against a sea of troubles,
    And by opposing end them? To die: to sleep;
    No more; and by a sleep to say we end
    The heart-ache and the thousand natural shocks
    That flesh is heir to, 'tis a consummation
    Devoutly to be wish'd. To die, to sleep;
    To sleep: perchance to dream: ay, there's the rub;
    For in that sleep of death what dreams may come
    When we have shuffled off this mortal coil,
    Must give us pause: there's the respect
    That makes calamity of so long life;
    For who would bear the whips and scorns of time,
    The oppressor's wrong, the proud man's contumely,
    The pangs of despised love, the law's delay,
    The insolence of office and the spurns
    That patient merit of the unworthy takes,
    When he himself might his quietus make
    With a bare bodkin? who would fardels bear,
    To grunt and sweat under a weary life,
    But that the dread of something after death,
    The undiscover'd country from whose bourn
    No traveller returns, puzzles the will
    And makes us rather bear those ills we have
    Than fly to others that we know not of?
    Thus conscience does make cowards of us all;
    And thus the native hue of resolution
    Is sicklied o'er with the pale cast of thought,
    And enterprises of great pith and moment
    With this regard their currents turn awry,
    And lose the name of action.
    """

    # Очистка и нормализация текста
    charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .,!?;:'\"\n-"
    cleaned_text = re.sub(f"[^{charset}]", "", hamlet_fragment)

    # Дублирование до 8000 символов
    multiplier = 8000 // len(cleaned_text) + 1
    full_text = (cleaned_text * multiplier)[:8000]

    return full_text


# 2. Статистический анализ
def analyze_text(text):
    """Анализ частот символов и биграмм"""
    # Частоты символов
    char_freq = Counter(text)

    # Частоты биграмм
    bigram_freq = defaultdict(int)
    for i in range(len(text) - 1):
        bigram = text[i:i + 2]
        bigram_freq[bigram] += 1

    # Сортировка результатов
    sorted_chars = sorted(char_freq.items(), key=lambda x: x[1], reverse=True)
    sorted_bigrams = sorted(bigram_freq.items(), key=lambda x: x[1], reverse=True)

    return sorted_chars, sorted_bigrams


# 3. Кодирование Хаффмана
class HuffmanNode:
    """Узел дерева Хаффмана"""

    def __init__(self, char=None, freq=0):
        self.char = char
        self.freq = freq
        self.left = None
        self.right = None

    def __lt__(self, other):
        return self.freq < other.freq


def build_huffman_tree(frequencies):
    """Построение дерева Хаффмана"""
    heap = [HuffmanNode(char, freq) for char, freq in frequencies]
    heapq.heapify(heap)

    while len(heap) > 1:
        left = heapq.heappop(heap)
        right = heapq.heappop(heap)

        merged = HuffmanNode(freq=left.freq + right.freq)
        merged.left = left
        merged.right = right

        heapq.heappush(heap, merged)

    return heap[0]


def generate_huffman_codes(node, prefix="", code_dict=None):
    """Генерация кодов Хаффмана"""
    if code_dict is None:
        code_dict = {}

    if node.char is not None:
        code_dict[node.char] = prefix
    else:
        generate_huffman_codes(node.left, prefix + "0", code_dict)
        generate_huffman_codes(node.right, prefix + "1", code_dict)

    return code_dict


def calculate_entropy(frequencies, total):
    """Расчет энтропии Шеннона"""
    entropy = 0.0
    for _, freq in frequencies:
        p = freq / total
        if p > 0:
            entropy -= p * math.log2(p)
    return entropy


# 4. Кодирование LZW
def lzw_compress(text):
    """Сжатие текста с помощью LZW"""
    # Инициализация словаря
    dictionary = {chr(i): i for i in range(256)}
    dict_size = 256

    w = ""
    result = []

    for c in text:
        wc = w + c
        if wc in dictionary:
            w = wc
        else:
            result.append(dictionary[w])
            # Добавляем новую последовательность
            dictionary[wc] = dict_size
            dict_size += 1
            w = c

    if w:
        result.append(dictionary[w])

    return result


def main():
    # Генерация текста
    text = generate_text()
    print(f"Длина текста: {len(text)} символов")
    print(f"Уникальных символов: {len(set(text))}")

    # Статистический анализ
    char_freq, bigram_freq = analyze_text(text)
    print("\nТоп-5 символов:")
    for char, freq in char_freq[:5]:
        print(f"'{char}': {freq} ({freq / len(text) * 100:.2f}%)")

    print("\nТоп-5 биграмм:")
    for bigram, freq in bigram_freq[:5]:
        print(f"'{bigram}': {freq} ({freq / (len(text) - 1) * 100:.2f}%)")

    # Кодирование Хаффмана
    root = build_huffman_tree(char_freq)
    huffman_codes = generate_huffman_codes(root)

    # Расчет размера после сжатия
    uniform_bits = len(text) * 6  # 6-битное равномерное кодирование
    huffman_bits = sum(len(huffman_codes[char]) * freq for char, freq in char_freq)

    # Расчет энтропии
    entropy = calculate_entropy(char_freq, len(text))
    avg_bits_per_char = huffman_bits / len(text)

    print("\nРезультаты Хаффмана:")
    print(f"Равномерное кодирование (6 бит): {uniform_bits} бит")
    print(f"Кодирование Хаффмана: {huffman_bits} бит")
    print(f"Экономия: {uniform_bits - huffman_bits} бит ({(uniform_bits - huffman_bits) / uniform_bits * 100:.1f}%)")
    print(f"Энтропия Шеннона: {entropy:.4f} бит/символ")
    print(f"Средняя длина кода Хаффмана: {avg_bits_per_char:.4f} бит/символ")
    print(f"Эффективность: {entropy / avg_bits_per_char * 100:.1f}% от теоретического предела")

    # Кодирование LZW
    compressed = lzw_compress(text)
    lzw_bits = len(compressed) * 12  # 12-битные коды

    print("\nРезультаты LZW:")
    print(f"Размер после LZW: {lzw_bits} бит")
    print(
        f"Экономия vs равномерного: {uniform_bits - lzw_bits} бит ({(uniform_bits - lzw_bits) / uniform_bits * 100:.1f}%)")
    print(
        f"Экономия vs Хаффмана: {huffman_bits - lzw_bits} бит ({(huffman_bits - lzw_bits) / huffman_bits * 100:.1f}%)")
    print(f"Коэффициент сжатия LZW: {len(text) * 8 / lzw_bits:.2f}:1 (исходные 8 бит/символ)")


if __name__ == "__main__":
    main()