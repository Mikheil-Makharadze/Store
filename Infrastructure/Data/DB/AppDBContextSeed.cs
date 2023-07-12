using Core.Entities;
using Core.Entities.Order;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.DB
{
    public class AppDBContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new List<Category>()
                    {
                        new Category
                        {
                            Name = "Action"
                        },
                        new Category
                        {
                            Name = "Adventure"
                        },
                        new Category
                        {
                            Name = "Role-playing"
                        },
                        new Category
                        {
                            Name = "Strategy"
                        },
                        new Category
                        {
                            Name = "Sports"
                        },
                        new Category
                        {
                            Name = "Puzzle"
                        },
                        new Category
                        {
                            Name = "Racing"
                        },
                        new Category
                        {
                            Name = "Shooter"
                        },
                        new Category
                        {
                            Name = "Horror"
                        },
                        new Category
                        {
                            Name = "Fighting"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Producers.Any())
                {
                    context.Producers.AddRange(new List<Producer>()
                    {
                        new Producer
                        {
                            Name = "Rockstar Games",
                            Description = "Rockstar Games is a game producer known for creating immersive open-world experiences" +
                            " with a focus on storytelling and player freedom. They have developed highly successful and " +
                            "critically acclaimed titles since their establishment",
                            Logo = "https://upload.wikimedia.org/wikipedia/commons/5/53/Rockstar_Games_Logo.svg"
                        },
                        new Producer
                        {
                            Name = "CD Projekt",
                            Description = "CD Projekt is a game producer known for its commitment to storytelling and player choice. They have developed notable games, " +
                            "especially in The Witcher series, and have gained a reputation for their attention to detail and rich narratives.",
                            Logo = "https://upload.wikimedia.org/wikipedia/en/6/68/CD_Projekt_logo.svg"
                        },
                        new Producer
                        {
                            Name = "Valve Corporation",
                            Description = "Valve Corporation is a game producer known for its innovative titles and influential contributions to the gaming industry." +
                            "They have developed successful games, often with a focus on multiplayer experience",
                            Logo = "https://upload.wikimedia.org/wikipedia/commons/a/ab/Valve_logo.svg"
                        },
                        new Producer
                        {
                            Name = "Blizzard Entertainment",
                            Description = "Blizzard Entertainment is a game producer renowned for its immersive multiplayer experiences and dedication to quality. " +
                            "They have developed several successful and beloved games.",
                            Logo = "https://upload.wikimedia.org/wikipedia/commons/2/23/Blizzard_Entertainment_Logo_2015.svg"
                        }
                        ,
                        new Producer
                        {
                            Name = "Electronic Arts (EA)",
                            Description = "EA is a major video game company that develops and publishes popular titles, including sports franchises like FIFA and Madden," +
                            " as well as acclaimed series such as Battlefield and The Sims.",
                            Logo = "https://upload.wikimedia.org/wikipedia/commons/8/81/Electronic_Arts_2020.svg"
                        }
                    });

                    context.SaveChanges();
                }
                
                if (!context.Product.Any())
                {
                    context.Product.AddRange(new List<Product>()
                    {
                        new Product
                        {
                            Name = "Grand Theft Auto V",
                            Description = "Grand Theft Auto V is an action-adventure game set in the fictional city of Los Santos. " +
                            "Players control three protagonists as they carry out various heists and missions in an open-world environment.",
                            Price = 39.99,
                            ProducerId = 1,
                            Image = "https://upload.wikimedia.org/wikipedia/en/a/a5/Grand_Theft_Auto_V.png"
                        },
                        new Product
                        {
                            Name = "Red Dead Redemption 2",
                            Description = " Red Dead Redemption 2 is an open-world action-adventure game set in the late 1800s. Players take on the role of Arthur Morgan, a member of an outlaw gang, as he navigates the vast landscapes and engages in various activities and missions.",
                            Price = 49.99,
                            ProducerId = 1,
                            Image = "https://upload.wikimedia.org/wikipedia/en/4/44/Red_Dead_Redemption_II.jpg"
                        },
                        new Product
                        {
                            Name = "Cyberpunk 2077",
                            Description = "Cyberpunk 2077 is a futuristic open-world role-playing game set in the dystopian Night City. Players assume the role of V, a mercenary seeking immortality, as they explore the city, interact with NPCs, and engage in combat and missions.",
                            Price = 59.99,
                            ProducerId = 2,
                            Image = "https://upload.wikimedia.org/wikipedia/en/9/9f/Cyberpunk_2077_box_art.jpg"
                        },
                        new Product
                        {
                            Name = "The Witcher 3: Wild Hunt",
                            Description = "The Witcher 3: Wild Hunt is an open-world action role-playing game based on the fantasy book series. Players control Geralt of Rivia, a monster hunter, as he embarks on a quest to find his adopted daughter and confronts various threats in a vast and richly detailed world.",
                            Price = 34.99,
                            ProducerId = 2,
                            Image = "https://upload.wikimedia.org/wikipedia/en/0/0c/Witcher_3_cover_art.jpg"
                        },
                        new Product
                        {
                            Name = "The Witcher 2: Assassins of Kings",
                            Description = "The Witcher 2: Assassins of Kings is the second installment in The Witcher series. It is an action role-playing game where players control Geralt of Rivia in his quest to uncover the secrets behind the assassination of a king.",
                            Price = 25.99,
                            ProducerId = 2,
                            Image = "https://upload.wikimedia.org/wikipedia/en/4/40/Witcher_2_cover.jpg"
                        },
                        new Product
                        {
                            Name = "The Witcher",
                            Description = "The Witcher is the first game in The Witcher series. It is an action role-playing game where players assume the role of Geralt of Rivia, a monster hunter with supernatural abilities, as he battles monsters and unravels a complex storyline.",
                            Price = 15.99,
                            ProducerId = 2,
                            Image = "https://upload.wikimedia.org/wikipedia/en/b/b0/The_Witcher_EU_box.jpg"
                        },
                        new Product
                        {
                            Name = "Dota 2",
                            Description = "Dota 2 is a multiplayer online battle arena (MOBA) game where two teams of five players compete to destroy each other's ancient structures. Players control unique heroes with various abilities and work together to outplay their opponents.",
                            Price = 0,
                            ProducerId = 3,
                            Image = "https://upload.wikimedia.org/wikipedia/en/3/31/Dota_2_Steam_artwork.jpg"
                        },
                        new Product
                        {
                            Name = "Portal 2",
                            Description = "Portal 2 is a puzzle-platform game that challenges players with solving spatial puzzles using a handheld portal device. It features a single-player campaign as well as cooperative multiplayer modes that require collaboration to overcome obstacles.",
                            Price = 17.99,
                            ProducerId = 3,
                            Image = "https://upload.wikimedia.org/wikipedia/en/f/f9/Portal2cover.jpg"
                        },
                        new Product
                        {
                            Name = "Counter-Strike: Global Offensive",
                            Description = "Counter-Strike: Global Offensive is a multiplayer first-person shooter game where players join either the terrorist or counter-terrorist team and engage in objective-based gameplay across various maps.",
                            Price = 0,
                            ProducerId = 3,
                            Image = "https://upload.wikimedia.org/wikipedia/en/6/6e/CSGOcoverMarch2020.jpg"
                        },
                        new Product
                        {
                            Name = "Overwatch",
                            Description = "Overwatch is a team-based first-person shooter game where players select from a diverse roster of heroes with unique abilities and engage in objective-based gameplay across various maps.",
                            Price = 33.99,
                            ProducerId = 4,
                            Image = "https://upload.wikimedia.org/wikipedia/commons/5/55/Overwatch_circle_logo.svg"
                        },
                        new Product
                        {
                            Name = "World of Warcraft",
                            Description = "World of Warcraft is a massively multiplayer online role-playing game (MMORPG) set in the fantasy world of Azeroth. Players create characters and explore the vast world, completing quests, battling monsters, and interacting with other players.",
                            Price = 0,
                            ProducerId = 4,
                            Image = "https://upload.wikimedia.org/wikipedia/en/6/65/World_of_Warcraft.png"
                        },
                        new Product
                        {
                            Name = "FIFA 23",
                            Description = "FIFA 23 is a sports simulation game that focuses on soccer. Players can take control of various teams and compete in matches, tournaments, or career modes, aiming to win championships and manage their team's progression.",
                            Price = 59.99,
                            ProducerId = 5,
                            Image = "https://upload.wikimedia.org/wikipedia/en/a/a6/FIFA_23_Cover.jpg"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Product_Categories.Any())
                {
                    context.Product_Categories.AddRange(new List<Product_Category>()
                    {
                        new Product_Category 
                        {
                            ProductId = 1,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 1,
                            CategoryId = 2,
                        },
                        new Product_Category
                        {
                            ProductId = 2,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 2,
                            CategoryId = 2,
                        },
                        new Product_Category
                        {
                            ProductId = 3,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 3,
                            CategoryId = 2,
                        },
                        new Product_Category
                        {
                            ProductId = 3,
                            CategoryId = 3,
                        },
                        new Product_Category
                        {
                            ProductId = 4,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 4,
                            CategoryId = 2,
                        },
                        new Product_Category
                        {
                            ProductId = 4,
                            CategoryId = 3,
                        },
                        new Product_Category
                        {
                            ProductId = 5,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 5,
                            CategoryId = 2,
                        },
                        new Product_Category
                        {
                            ProductId = 5,
                            CategoryId = 3,
                        },
                        new Product_Category
                        {
                            ProductId = 6,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 6,
                            CategoryId = 2,
                        },
                        new Product_Category
                        {
                            ProductId = 6,
                            CategoryId = 3,
                        },
                        new Product_Category
                        {
                            ProductId = 7,
                            CategoryId = 4,
                        },
                        new Product_Category
                        {
                            ProductId = 8,
                            CategoryId = 1,
                        },
                        new Product_Category
                        {
                            ProductId = 8,
                            CategoryId = 6,
                        },
                        new Product_Category
                        {
                            ProductId = 9,
                            CategoryId = 8,
                        },
                        new Product_Category
                        {
                            ProductId = 10,
                            CategoryId = 9,
                        },
                        new Product_Category
                        {
                            ProductId = 11,
                            CategoryId = 3,
                        },
                        new Product_Category
                        {
                            ProductId = 11,
                            CategoryId = 4,
                        },
                        new Product_Category
                        {
                            ProductId = 12,
                            CategoryId = 5,
                        }
                    });

                    context.SaveChanges();
                }

                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(new List<Order>()
                        {
                            new Order
                            {
                                UserEmail = "customer@store.com",
                                OrderDate = DateTime.Now.AddDays(-3),
                                Subtotal = 59.99,
                                Status = OrderStatus.Completed
                            },
                            new Order
                            {
                                UserEmail = "customer@store.com",
                                OrderDate = DateTime.Now.AddDays(-1),
                                Subtotal = 89.98,
                                Status = OrderStatus.Processing
                            },
                            new Order
                            {
                                UserEmail = "customer@store.com",
                                OrderDate = DateTime.Now,
                                Subtotal = 77.97,
                                Status = OrderStatus.Delivered
                            },
                            new Order
                            {
                                UserEmail = "mixa@store.com",
                                OrderDate = DateTime.Now.AddDays(-3),
                                Subtotal = 25.95,
                                Status = OrderStatus.Shipped
                            },
                            new Order
                            {
                                UserEmail = "mixa@store.com",
                                OrderDate = DateTime.Now.AddDays(-1),
                                Subtotal = 0,
                                Status = OrderStatus.Pending
                            }
                        });

                    context.SaveChanges();

                    context.OrderItems.AddRange(new List<OrderItem>
                        {
                            new OrderItem
                            {
                                OrderId = 1,
                                Amount = 1,
                                ProductId = 3,
                            },
                            new OrderItem
                            {
                                OrderId = 2,
                                ProductId = 1,
                                Amount = 1,
                            },
                            new OrderItem
                            {
                                OrderId = 2,
                                ProductId = 2,
                                Amount = 1,
                            },
                            new OrderItem
                            {
                                OrderId = 3,
                                ProductId = 4,
                                Amount = 1,
                            },
                            new OrderItem
                            {
                                OrderId = 3,
                                ProductId = 5,
                                Amount = 1,
                            },
                            new OrderItem
                            {
                                OrderId = 3,
                                ProductId = 6,
                                Amount = 1,
                            },
                            new OrderItem
                            {
                                OrderId = 4,
                                ProductId = 8,
                                Amount = 2,
                            },
                            new OrderItem
                            {
                                OrderId = 5,
                                ProductId = 9,
                                Amount = 1,
                            },
                            new OrderItem
                            {
                                OrderId = 5,
                                ProductId = 11,
                                Amount = 1,
                            }
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}
