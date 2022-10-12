### Рефакторинг неприжившийся структуры
---

Задача: изменить стутуру класса 

Было принято решение изменить структуру хранения приборов и их детекторов.
Для этого нужно создать новые таблицы и перераспределить старые данные.
Так как класс приборов использовался раньше, необходимо сохранить все прежние методы взаимодействия.


```
Использовал:
  .Net 4.0
  Odbc
```

Изменения в бд удобнее было сделать без использования C#, поэтому запросы сохранил в текстовые файлы.

***На предприятии остались устройства под Windows XP, поэтому использую .Net 4.0***