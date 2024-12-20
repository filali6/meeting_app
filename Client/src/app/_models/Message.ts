export interface Message {
    id: number
    sourcePhotoUrl: string
    sourceUsername: string
    sourceUserId: string
    targetPhotoUrl: string
    targetUsername: string
    targetUserId: string
    content: string
    readDate?: Date
    sentDate: Date
  }