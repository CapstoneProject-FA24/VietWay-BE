using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.Migrations;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class BookingRefundService(IUnitOfWork unitOfWork, IIdGenerator idGenerator) : IBookingRefundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task<PaginatedList<BookingRefundDTO>> GetBookingRefundAsync(string? bookingId, RefundStatus? refundStatus, int pageSize, int pageIndex)
        {
            IQueryable<BookingRefund> query = _unitOfWork.BookingRefundRepository.Query()
                .OrderByDescending(x => x.CreatedAt);
            if (bookingId != null)
            {
                query = query.Where(x => x.BookingId == bookingId);
            }
            if (refundStatus != null)
            {
                query = query.Where(x => x.RefundStatus == refundStatus);
            }
            int count = await query.CountAsync();
            List<BookingRefundDTO> bookingRefundDTOs = await query
                .Skip(pageSize * (pageIndex-1))
                .Take(pageSize)
                .Select(x => new BookingRefundDTO
                {
                    RefundId = x.RefundId,
                    BookingId = x.BookingId,
                    RefundAmount = x.RefundAmount,
                    RefundStatus = x.RefundStatus,
                    RefundDate = x.RefundDate,
                    RefundReason = x.RefundReason,
                    RefundNote = x.RefundNote,
                    BankCode = x.BankCode,
                    BankTransactionNumber = x.BankTransactionNumber,
                    CreatedAt = x.CreatedAt,
                })
                .ToListAsync();
            return new PaginatedList<BookingRefundDTO>
            {
                Items = bookingRefundDTOs,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count
            };
        }

        public async Task UpdateBookingRefundInfo(string accountId, string refundId, BookingRefund bookingRefund)
        {
            Account account = await _unitOfWork.AccountRepository.Query().SingleOrDefaultAsync(x => x.AccountId == accountId);
            if (account.Role != UserRole.Manager && account.Role != UserRole.Staff)
            {
                throw new UnauthorizedException("UNAUTHORIZED");
            }

            BookingRefund pendingBookingRefund = await _unitOfWork
                .BookingRefundRepository.Query()
                .Include(x => x.Booking)
                .SingleOrDefaultAsync(x => x.RefundId == refundId && x.RefundStatus == RefundStatus.Pending)
                ?? throw new ResourceNotFoundException("NOT_EXIST_BOOKING_REFUND");
            pendingBookingRefund.RefundNote = bookingRefund.RefundNote;
            pendingBookingRefund.RefundStatus = RefundStatus.Refunded;
            pendingBookingRefund.BankCode = bookingRefund.BankCode;
            pendingBookingRefund.RefundDate = bookingRefund.RefundDate;
            pendingBookingRefund.BankTransactionNumber = bookingRefund.BankTransactionNumber;
            string entityHistoryId = _idGenerator.GenerateId();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.BookingRefundRepository.UpdateAsync(pendingBookingRefund);
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new()
                {
                    Action = EntityModifyAction.Update,
                    EntityId = refundId,
                    EntityType = EntityType.BookingRefund,
                    Id = entityHistoryId,
                    ModifiedBy = accountId,
                    ModifierRole = account.Role,
                    Reason = bookingRefund.RefundNote,
                    StatusHistory = new EntityStatusHistory
                    {
                        Id = entityHistoryId,
                        NewStatus = (int)RefundStatus.Refunded,
                        OldStatus = (int)RefundStatus.Pending,
                    }
                });
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
