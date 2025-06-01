import logging
import random
from aiogram import Bot, Dispatcher, types, F
from aiogram.filters.command import Command
from aiogram.utils.keyboard import InlineKeyboardBuilder

logging.basicConfig(level=logging.INFO)

bot = Bot(token="")  # свой токен сюда
dp = Dispatcher()

stats = {}

@dp.message(Command("start"))
async def cmd_start(message: types.Message):
    await message.answer(
        "Привет! Я бот для игры в 'Камень, ножницы, бумага'.\n\n"
        "Используй команды:\n"
        "/play - начать игру\n"
        "/stats - показать статистику"
    )


@dp.message(Command("stats"))
async def cmd_stats(message: types.Message):
    user_id = message.from_user.id
    user_stats = stats.get(user_id, {"wins": 0, "losses": 0, "draws": 0})

    await message.answer(
        f"📊 Ваша статистика:\n"
        f"🏆 Побед: {user_stats['wins']}\n"
        f"💥 Поражений: {user_stats['losses']}\n"
        f"🤝 Ничьих: {user_stats['draws']}"
    )


@dp.message(Command("play"))
async def cmd_play(message: types.Message):
    builder = InlineKeyboardBuilder()
    buttons = [
        ("Камень", "rock"),
        ("Ножницы", "scissors"),
        ("Бумага", "paper")
    ]

    for text, data in buttons:
        builder.button(text=text, callback_data=data)
    builder.adjust(3)

    await message.answer(
        "Выбери свой ход:",
        reply_markup=builder.as_markup()
    )


@dp.callback_query(F.data.in_(["rock", "scissors", "paper"]))
async def process_choice(callback: types.CallbackQuery):
    user_choice = callback.data
    bot_choice = random.choice(["rock", "scissors", "paper"])

    # Определение победителя
    result = determine_winner(user_choice, bot_choice)

    # Обновление статистики
    user_id = callback.from_user.id
    if user_id not in stats:
        stats[user_id] = {"wins": 0, "losses": 0, "draws": 0}

    if result == "win":
        stats[user_id]["wins"] += 1
        result_text = "Ты победил! �️"
    elif result == "lose":
        stats[user_id]["losses"] += 1
        result_text = "Бот победил! 🤖"
    else:
        stats[user_id]["draws"] += 1
        result_text = "Ничья! 🤝"

    # Отображение выбора
    choices = {
        "rock": "Камень 🪨",
        "scissors": "Ножницы ✂️",
        "paper": "Бумага 📜"
    }

    await callback.message.edit_text(
        f"Твой выбор: {choices[user_choice]}\n"
        f"Выбор бота: {choices[bot_choice]}\n\n"
        f"{result_text}",
        reply_markup=None
    )
    await callback.answer()


def determine_winner(user, bot):
    if user == bot:
        return "draw"

    win_conditions = {
        "rock": "scissors",
        "scissors": "paper",
        "paper": "rock"
    }

    return "win" if win_conditions[user] == bot else "lose"


if __name__ == "__main__":
    dp.run_polling(bot)