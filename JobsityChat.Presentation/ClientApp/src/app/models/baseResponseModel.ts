export class BaseResponseModel<T> {
    response: T;
    success: boolean;
    message: string;
}