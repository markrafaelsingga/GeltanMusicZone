INSERT INTO [dbo].[supplier] (
    [sup_company], [sup_fname], [sup_mi], [sup_lname], [sup_address], [sup_email], [sup_phone]
)
VALUES
    ('Music Supplier Co.', 'John', 'D', 'Doe', '123 Main St, Cityville', 'john.doe@example.com', '555-1234'),
    ('Instrument Warehouse', 'Jane', 'A', 'Smith', '456 Oak St, Townsville', 'jane.smith@example.com', '555-5678'),
    ('Audio Gear Ltd.', 'Bob', 'M', 'Johnson', '789 Maple St, Villagetown', 'bob.johnson@example.com', '555-9876');

-- Insert sample data into the product table
INSERT INTO [dbo].[product] ([prod_name], [prod_desc], [prod_price], [sup_id])
VALUES
    ('Electric Guitar', 'Classic six-string electric guitar', 599.99, 1),
    ('Acoustic Guitar', 'Dreadnought acoustic guitar with solid spruce top', 499.99, 2),
    ('Drum Set', '5-piece drum set with cymbals and hardware', 799.99, 3),
    ('Keyboard', '88-key weighted digital piano with built-in speakers', 899.99, 1),
    ('Violin', '4/4 size violin with bow and case', 299.99, 2),
    ('Saxophone', 'Alto saxophone with brass body and lacquer finish', 749.99, 3),
    ('Amplifier', '20-watt guitar amplifier with distortion', 149.99, 1),
    ('Microphone', 'Condenser microphone with shock mount and pop filter', 79.99, 2),
    ('Bass Guitar', '4-string bass guitar with solid body', 549.99, 3),
    ('Digital Mixer', '16-channel digital audio mixer with USB interface', 1299.99, 1);