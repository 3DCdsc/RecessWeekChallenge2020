using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _3DC.RecessWeekChallenge.Data;
using _3DC.RecessWeekChallenge.Models;
using Microsoft.AspNetCore.Authorization;

namespace _3DC.RecessWeekChallenge.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly _3DCRecessWeekChallengeContext _context;

        public LeaderboardController(_3DCRecessWeekChallengeContext context)
        {
            _context = context;
        }

        // GET: LeaderboardRows
        public async Task<IActionResult> Index()
        {
            var participants = from p in _context.LeaderboardRow select p;
            participants = participants
                .OrderByDescending(p => p.LabScore + p.HackerrankScore)
                .ThenBy(p => p.HackerrankTimeInt);
            List<LeaderboardRow> participantsResult = await participants.AsNoTracking().ToListAsync();
            for (int i = 0; i < participantsResult.Count(); ++i)
            {
                participantsResult[i].Rank = i + 1;
            }
            return View(participantsResult);
        }

        // GET: LeaderboardRows/Details/5
        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboardRow = await _context.LeaderboardRow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaderboardRow == null)
            {
                return NotFound();
            }

            return View(leaderboardRow);
        }

        // GET: LeaderboardRows/Create
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaderboardRows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([Bind("Id,Name,HackerrankUsername,LabScore,HackerrankScore")] LeaderboardRow leaderboardRow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaderboardRow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaderboardRow);
        }

        // GET: LeaderboardRows/Edit/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboardRow = await _context.LeaderboardRow.FindAsync(id);
            if (leaderboardRow == null)
            {
                return NotFound();
            }
            return View(leaderboardRow);
        }

        // POST: LeaderboardRows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HackerrankUsername,LabScore,HackerrankScore")] LeaderboardRow leaderboardRow)
        {
            if (id != leaderboardRow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaderboardRow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaderboardRowExists(leaderboardRow.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaderboardRow);
        }

        // GET: LeaderboardRows/Delete/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboardRow = await _context.LeaderboardRow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaderboardRow == null)
            {
                return NotFound();
            }

            return View(leaderboardRow);
        }

        // POST: LeaderboardRows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaderboardRow = await _context.LeaderboardRow.FindAsync(id);
            _context.LeaderboardRow.Remove(leaderboardRow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaderboardRowExists(int id)
        {
            return _context.LeaderboardRow.Any(e => e.Id == id);
        }
    }
}
