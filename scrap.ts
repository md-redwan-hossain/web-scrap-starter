import Database from "better-sqlite3";
import { drizzle } from "drizzle-orm/better-sqlite3";
import puppeteerExtra from "puppeteer-extra";
import stealthPlugin from "puppeteer-extra-plugin-stealth";
import { rokomari_data } from "./drizzle/schema";

const sqlite = new Database("db.sqlite3");
const db = drizzle(sqlite);

type BookData = {
  book_title: string;
  author_name: string;
};

async function scrap(pageNumber: number): Promise<BookData[]> {
  puppeteerExtra.use(stealthPlugin());
  const browser = await puppeteerExtra.launch({ headless: false });
  const page = await browser.newPage();
  await page.setViewport({ width: 1080, height: 1024 });
  const baseUrl = "https://www.rokomari.com/book/category/81/islamic";
  const query = `${baseUrl}?page=${pageNumber}`;

  try {
    await page.goto(query);
  } catch (error) {
    console.log("Error going to page");
  }

  const target = ".books-wrapper";
  await page.waitForSelector(target);

  const data = await page.evaluate(() => {
    let dataFromDom: Array<BookData> = [];

    const booksWrapper = document.querySelector(".books-wrapper");

    if (booksWrapper) {
      const bookDivs = booksWrapper.querySelectorAll(".books-wrapper__item");

      bookDivs.forEach((bookDiv) => {
        const bookAuthorElement = bookDiv.querySelector(".book-author");
        const bookAuthor = bookAuthorElement ? bookAuthorElement?.textContent?.trim() : "N/A";

        const bookTitleElement = bookDiv.querySelector(".book-title");
        const bookTitle = bookTitleElement ? bookTitleElement?.textContent?.trim() : "N/A";

        if (bookAuthor && bookTitle) {
          dataFromDom.push({ book_title: bookTitle, author_name: bookAuthor });
        }
      });
    }
    return dataFromDom;
  });
  await browser.close();
  return data;
}

const pageNumberLowerLimit = 1;
const pageNumberHigherLimit = 5;
// const pageNumberHigherLimit = 166;

async function insertData(dataForInert: Array<BookData>) {
  await db.insert(rokomari_data).values(dataForInert).onConflictDoNothing();
}

async function runLoop() {
  const looper: Array<number> = [];

  for (let i = pageNumberLowerLimit; i <= pageNumberHigherLimit; i++) {
    looper.push(i);
  }

  for (const i of looper) {
    const dataForDb = await scrap(i);
    await insertData(dataForDb);
  }
}

(async () => {
  await runLoop();
  console.log("Done");
  process.exit();
})();
