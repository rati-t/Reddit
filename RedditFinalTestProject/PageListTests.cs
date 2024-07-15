using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.Models;
using Reddit.Repositories;
using System.Drawing.Printing;
using System.Security.Policy;

namespace RedditFinalTestProject
{
    public class PageListTests
    {
        private int SeededItemsCount = 781;
        private PostsRepository GetPostsRepository(bool seed = true)
        {
            var databaseName = Guid.NewGuid().ToString();
            var databaseOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName).Options;

            var databaseContext = new ApplicationDbContext(databaseOptions);

            if (seed)
            {
                var posts = new List<Post>();
                for (int i = 0; i < SeededItemsCount; i++)
                {
                    posts.Add(new Post()
                    {
                        Title = $"Title{i}",
                        Content = $"Content{i}",
                        Downvotes = i,
                        Upvotes = i * 10
                    });
                }
                databaseContext.AddRange(posts);
                databaseContext.SaveChanges();
            }
            return new PostsRepository(databaseContext);
        }

        [Fact]
        public async Task PagedList_GetRightItems()
        {
            var page = 32;
            var pageSize = 10;

            var repo = GetPostsRepository();

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.Equal(items.Items.Count, pageSize);
            var counter = 0;
            foreach (var item in items.Items)
            {
                Assert.True(item.Downvotes == (page - 1) * pageSize + counter);
                counter++;
            }
        }

        [Fact]
        public async Task PagedList_HasNextPage()
        {
            var page = 32;
            var pageSize = 10;

            var repo = GetPostsRepository();

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.True(items.HasPreviousPage);
        }


        [Fact]
        public async Task PagedList_HasPreviousePage()
        {
            var page = 32;
            var pageSize = 10;

            var repo = GetPostsRepository();

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.True(items.HasPreviousPage);
        }

        [Fact]
        public async Task PageList_WhenListIsEmpty()
        {
            var page = 32;
            var pageSize = 10;

            var repo = GetPostsRepository(false);

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.True(items.Items.Count == 0);
        }

        [Fact]
        public async Task PageList_PageSizeIsMoreThanItemCount()
        {
            var page = 32;
            var pageSize = SeededItemsCount + 100;

            var repo = GetPostsRepository();

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.True(items.Items.Count == 0);
        }

        [Fact]
        public async Task PageList_TotalCountIsMoreThanPageSize()
        {
            var page = 32;
            var pageSize = 10;

            var repo = GetPostsRepository();

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.True(items.Items.Count == pageSize);
        }

        [Fact]
        public async Task PageList_PageIsNonPositive()
        {
            var page = -1;
            var pageSize = 10;

            var repo = GetPostsRepository();

            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetPosts(page, pageSize, null, null, null));

            Assert.True(ex.ParamName == nameof(page));
        }

        [Fact]
        public async Task PageList_PageSizeIsNonPositive()
        {
            var page = 32;
            var pageSize = -1;

            var repo = GetPostsRepository();

            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetPosts(page, pageSize, null, null, null));

            Assert.True(ex.ParamName == nameof(pageSize));
        }

        [Fact]
        public async Task PageList_PageIsOutOfRange()
        {
            var page = 32;
            var pageSize = 100;

            var repo = GetPostsRepository();

            var items = await repo.GetPosts(page, pageSize, null, null, null);

            Assert.True(items.Items.Count == 0);
        }
    }
}