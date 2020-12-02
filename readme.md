## Intro

上傳圖片檔案後，壓縮並存入 mariaDB

檢視時，將資料取出解壓後轉 base64 顯示圖片

## MariaDB

### Docker

```yml
IntelliJ IDEA
version: '3'

services:
    mariadb:
        image: 'docker.io/bitnami/mariadb:10.5-debian-10'
        restart: always
        container_name: mariadb
        ports:
            - '3306:3306'
        environment:
            - ALLOW_EMPTY_PASSWORD=yes
            - MARIADB_ROOT_PASSWORD=password
            - MARIADB_USER=my_user
            - MARIADB_PASSWORD=my_password
            - MARIADB_DATABASE=my_database
```

### Table

```sql
create or replace table my_database.my_image
(
    id       int auto_increment
        primary key,
    data     longblob null,
    `before` double   null,
    after    double   null
);

```

### SP

```sql
create or replace
    definer = root@`%` procedure my_database.usp_myimage_create(OUT Id int, IN my_data longblob, IN my_before double, IN my_after double)
BEGIN
    INSERT INTO my_image (data, `before`, after) values (my_data, my_before, my_after);
    SET Id = @@IDENTITY;
END;


create or replace
    definer = root@`%` procedure my_database.usp_myimage_get(IN my_id int)
BEGIN
    select id,
           data,
           `before`,
           after,
           ROUND(after / `before` * 100, 2) as rate
    from my_image
    where id = my_id;
END;

create or replace
    definer = root@`%` procedure my_database.usp_myimage_getall()
BEGIN
    select id,
           data,
           `before`,
           after,
           ROUND(after / `before` * 100, 2) as rate
    from my_image;
END;

```

### Connection String

```
server=127.0.0.1;port=3306;user id=root;password=password;database=my_database;charset=utf8;ConnectionTimeout=3;
```
