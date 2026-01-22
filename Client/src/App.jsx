import React, { useState, useEffect } from 'react'
import axios from 'axios'
import './App.css'

const API_URL = 'http://localhost:5000/api/token/alpha-tokens'

function App() {
  const [tokens, setTokens] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [activeTab, setActiveTab] = useState('active') // 'active' hoặc 'expired'

  useEffect(() => {
    fetchTokens()
    // Refresh mỗi 5 phút
    const interval = setInterval(fetchTokens, 5 * 60 * 1000)
    return () => clearInterval(interval)
  }, [])

  const fetchTokens = async () => {
    try {
      setLoading(true)
      setError(null)
      const response = await axios.get(API_URL)
      setTokens(response.data)
    } catch (err) {
      console.error('Error fetching tokens:', err)
      setError('Không thể tải dữ liệu từ server. Vui lòng kiểm tra kết nối.')
    } finally {
      setLoading(false)
    }
  }

  const formatDate = (dateString) => {
    const date = new Date(dateString)
    return date.toLocaleDateString('vi-VN', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    })
  }

  if (loading) {
    return (
      <div className="app-container">
        <div className="loading">Đang tải dữ liệu...</div>
      </div>
    )
  }

  if (error) {
    return (
      <div className="app-container">
        <div className="error">{error}</div>
        <button onClick={fetchTokens} className="retry-button">
          Thử lại
        </button>
      </div>
    )
  }

  // Filter tokens theo tab và sắp xếp
  const activeTokens = tokens
    .filter(token => !token.isExpired && token.daysRemaining > 0)
    .sort((a, b) => b.daysRemaining - a.daysRemaining) // Sắp xếp theo số ngày còn lại giảm dần (nhiều tới ít)
  
  const expiredTokens = tokens
    .filter(token => token.isExpired && token.daysRemaining >= -3)
    .sort((a, b) => b.daysRemaining - a.daysRemaining) // Sắp xếp theo số ngày (token vừa hết hạn ở trên)

  const displayedTokens = activeTab === 'active' ? activeTokens : expiredTokens

  return (
    <div className="app-container">
      <div className="header">
        <h1>Alpha Binance Token List</h1>
        <p className="subtitle">Danh sách token mới list trong 30 ngày gần đây</p>
      </div>

      {/* Tabs */}
      <div className="tabs">
        <button
          className={`tab ${activeTab === 'active' ? 'active' : ''}`}
          onClick={() => setActiveTab('active')}
        >
          Token còn hạn ({activeTokens.length})
        </button>
        <button
          className={`tab ${activeTab === 'expired' ? 'active' : ''}`}
          onClick={() => setActiveTab('expired')}
        >
          Token hết hạn &lt; 3 ngày ({expiredTokens.length})
        </button>
      </div>

      <div className="token-list">
        {displayedTokens.length === 0 ? (
          <div className="empty-state">
            {activeTab === 'active' 
              ? 'Không có token nào còn hạn trong khoảng thời gian này.'
              : 'Không có token nào hết hạn dưới 3 ngày.'}
          </div>
        ) : (
          displayedTokens.map((token, index) => (
            <div key={index} className={`token-card ${token.isExpired ? 'expired' : ''}`}>
              <div className="token-info">
                <div className="token-name">
                  <span className="name">{token.name}</span>
                  <span className="symbol">({token.symbol})</span>
                </div>
                <div className={`days-remaining ${token.isExpired ? 'expired' : ''}`}>
                  <span className="days-number">
                    {token.isExpired ? Math.abs(token.daysRemaining) : token.daysRemaining}
                  </span>
                  <span className="days-label">
                    {token.isExpired 
                      ? `ngày đã hết hạn`
                      : token.daysRemaining === 1 
                        ? 'ngày còn lại' 
                        : 'ngày còn lại'}
                  </span>
                </div>
              </div>
              <div className="listing-date">
                List: {formatDate(token.listingDate)}
              </div>
            </div>
          ))
        )}
      </div>

      <div className="footer">
        <button onClick={fetchTokens} className="refresh-button">
          🔄 Làm mới
        </button>
        <div className="time-info">
          Thời gian hiện tại (Hà Nội): {new Date().toLocaleString('vi-VN', { timeZone: 'Asia/Ho_Chi_Minh' })}
        </div>
      </div>
    </div>
  )
}

export default App
