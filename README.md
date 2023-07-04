# Шейдер для корректной отрисовки градиента в UI.

## Задача

- Получить визуально корректный градиент.
- Кнопки разных цветов.
- Рамки цвета градиента рамки задаются отдельно от основного градиента.
- Экономия текстурного пространства в текстурном атласе.

## Решение:
Написан специальный шейдер UI-GradientImage. Который работает с текстурной маской градиента. В параметре G пикселей этой текстуры задается сила градиента от цвета 1 до цвета 2. Отдельно используют для фона и рамки.

## Результат
Получаем возможность задавать любые сочетание визуально корректного градиента цветов при экономии пространства в текстурном атласе. 

Тест
Алиса 1
бррррр