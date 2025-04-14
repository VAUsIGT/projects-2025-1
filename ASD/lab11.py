# проверяет, можно ли назначить цвет вершине без конфликтов
def is_safe(graph, vertex, color_map, color):
    for neighbor in graph[vertex]:
        if color_map[neighbor] == color:
            return False
    return True

# рекурсивно пытается раскрасить вершины, начиная с указанного индекса
def backtrack(graph, vertices, k, color_map, index):
    if index == len(vertices):
        return True
    current_vertex = vertices[index]
    for color in range(k):
        if is_safe(graph, current_vertex, color_map, color):
            color_map[current_vertex] = color
            if backtrack(graph, vertices, k, color_map, index + 1):
                return True
            color_map[current_vertex] = -1  # Откат
    return False

# находит минимальное количество цветов для раскраски графа и саму раскраску
def graph_coloring(graph):
    if not graph:
        return 0, {}
    # сортируем вершины по убыванию степени для оптимизации
    vertices = sorted(graph.keys(), key=lambda v: len(graph[v]), reverse=True)
    max_degree = max(len(adj) for adj in graph.values())
    # перебираем возможные k от 1 до max_degree + 1
    for k in range(1, max_degree + 2):
        color_map = {v: -1 for v in vertices}
        if backtrack(graph, vertices, k, color_map, 0):
            return k, color_map
    return None  # недостижимо

if __name__ == "__main__":
    graph = {
        'A': ['B', 'C'],
        'B': ['A', 'D'],
        'C': ['A', 'D'],
        'D': ['B', 'C']
    }
    k, colors = graph_coloring(graph)
    print(f"Хроматическое число: {k}")
    print("Раскраска вершин:")
    for vertex, color in colors.items():
        print(f"{vertex}: {color}")