CREATE TABLE users (
    user_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username TEXT NOT NULL,
    city TEXT NOT NULL
);

CREATE TABLE status (
    status_id SERIAL PRIMARY KEY,
    status_name TEXT NOT NULL
);

CREATE TABLE orders (
    order_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID,
    order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    total_amount DECIMAL(10, 2) NOT NULL,
    status_id INT,
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (status_id) REFERENCES status(status_id)
);

INSERT INTO public.status (status_name)
VALUES
    ('В обработке'),
    ('Выполнено'),
    ('Отменено');